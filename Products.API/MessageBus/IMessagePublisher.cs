using Products.API.Models;

namespace Products.API.MessageBus
{
    public interface IMessagePublisher
    {
        Task PublishMessageAsync(EventDto publishEventDto);
    }
}
