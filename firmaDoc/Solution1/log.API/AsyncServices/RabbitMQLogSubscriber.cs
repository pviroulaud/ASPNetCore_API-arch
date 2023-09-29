using log.API.Repositories;
using logEntities.logModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace log.API.AsyncServices
{
    public class RabbitMQLogSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private string _exchangeName ="log";
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogRepository _context;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public RabbitMQLogSubscriber(IConfiguration configuration,
            IEventProcessor eventProcessor,
            ILogRepository context
            )
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _context = context;


            InitializeRabbitMQ();

        }
        private void InitializeRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName,
                    exchange: _exchangeName,
                    routingKey: "");


                _context.addLog(new operationLog()
                {
                    operationId = 1,
                    operationDate = DateTime.Now,
                    userId = 0,
                    entity = "RabbitMQ",
                    description = $"Connection. RabbitMQHost: {_configuration["RabbitMQHost"]}, RabbitMQPort: {int.Parse(_configuration["RabbitMQPort"])} Exchange:{_exchangeName}"
                });

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {
                _context.addErrorLog(new errorLog()
                {
                    errorCode = 0,
                    errorDate = DateTime.Now,
                    userId = 0,
                    detail = ex.Message,
                    location = ex.StackTrace,
                    _params = ""
                });

            }

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _context.addLog(new operationLog()
                {
                    operationId = 1,
                    operationDate = DateTime.Now,
                    userId = 0,
                    entity = "RabbitMQ",
                    description = $"Event. Exchange:{_exchangeName}, Message: {notificationMessage}"
                });

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _context.addLog(new operationLog()
            {
                operationId = 1,
                operationDate = DateTime.Now,
                userId = 0,
                entity = "RabbitMQ",
                description = $"Connection Shutdown. {e.ReplyText}"
            });
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
