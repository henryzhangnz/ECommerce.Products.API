using Products.API.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Products.API.MessageBus
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConfiguration _config;
        private readonly Task<IConnection> _connection;
        private readonly Task<IChannel> _channel;

        public MessagePublisher(IConfiguration configuration)
        {
            _config = configuration;
            var factory = new ConnectionFactory
            {
                HostName = _config["Rabbitmq:Host"],
                Port = int.Parse(_config["Rabbitmq:Port"])
            };
            try
            {
                _connection = factory.CreateConnectionAsync();
                _channel = _connection.Result.CreateChannelAsync();

                _channel.Result.ExchangeDeclareAsync(exchange: _config["Rabbitmq:Exchange"], type: ExchangeType.Fanout);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot connect to RabbitMQ");
            }
        }
        public async Task PublishMessageAsync(EventDto publishEventDto)
        {
            var message = JsonSerializer.Serialize(publishEventDto);
            if (_connection.Result.IsOpen)
            {
                await SendMessage(message);
                Console.WriteLine("--> Event Sent!");
            }
        }

        private async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            await _channel.Result.BasicPublishAsync(
                exchange: _config["Rabbitmq:Exchange"],
                routingKey: "",
                body: body);
        }
    }
}
