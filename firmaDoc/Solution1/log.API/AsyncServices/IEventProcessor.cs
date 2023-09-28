namespace log.API.AsyncServices
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
