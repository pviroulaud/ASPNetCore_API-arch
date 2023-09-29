using fileDTO;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace fileAPI.AsyncServices
{
    public class RabbitMQClient<T> : IRabbitMQClient<T>
    {
        private readonly IConfiguration _configuration;
        private readonly string _exchangeName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClient(IConfiguration configuration,string exchangeName)
        {
            _configuration = configuration;
            _exchangeName = exchangeName;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = Convert.ToInt32(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
                

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
        public void sendMessage(T msg)
        {
            if (_connection.IsOpen)
            {
                var message = JsonSerializer.Serialize(msg);

                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: _exchangeName,
                                routingKey: "",
                                basicProperties: null,
                                body: body);
            }
            
        }


        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
