namespace fileAPI.AsyncServices
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
