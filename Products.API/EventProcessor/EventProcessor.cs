using Products.API.Models;
using Products.API.Repositories;
using System.Text.Json;

namespace Products.API.EventProcessor
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ProcessEventAsync(string message)
        {
            var eventData = JsonSerializer.Deserialize<EventDto>(message);

            using (var scope = _scopeFactory.CreateScope())
            {
                var orderRepo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                switch (Enum.GetName(eventData.EventType))
                {
                    case "OrderUpdated":
                        Console.WriteLine($"{EventType.OrderUpdated.ToString()} received with data: {eventData.Data} for id: {eventData.Id}");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
