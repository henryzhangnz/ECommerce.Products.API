using Products.API.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Products.API.MessageBus
{
    public class MessageBusClient : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IChannel _channel;
        private string _queue;

        public MessageBusClient(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeRabbitMQ();

            // Call base StartAsync if necessary
            await base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (ModuleHandle, ea) =>
            {
                try
                {
                    Console.WriteLine("--> Event Received!");

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());

                    await _eventProcessor.ProcessEventAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Error processing event: {ex.Message}");
                }
            };

            _channel.BasicConsumeAsync(queue: _queue, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = _configuration["Rabbitmq:Host"], Port = int.Parse(_configuration["Rabbitmq:Port"]) };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: _configuration["Rabbitmq:Exchange"], type: ExchangeType.Fanout);
            _queue = await _channel.QueueDeclareAsync();
            await _channel.QueueBindAsync(queue: "",
                exchange: _configuration["Rabbitmq:Exchange"],
                routingKey: "");
        }
    }
}