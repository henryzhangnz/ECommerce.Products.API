namespace Products.API.EventProcessor
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message);
    }
}
