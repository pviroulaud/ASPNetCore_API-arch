
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace fileAPI.AsyncServices
{
    public class RabbitMQFileSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private string _exchangeName = "file";
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public RabbitMQFileSubscriber(IConfiguration configuration,
            IEventProcessor eventProcessor
            )
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;


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


                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {

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

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {

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
