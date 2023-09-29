namespace documentAPI.AsyncServices
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
