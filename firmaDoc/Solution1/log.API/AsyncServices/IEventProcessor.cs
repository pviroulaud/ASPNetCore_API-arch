namespace log.API.AsyncServices
{
    public interface IEventProcessor<T>
    {
        void ProcessEvent(string message);
    }
}
