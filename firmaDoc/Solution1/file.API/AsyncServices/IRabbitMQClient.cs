
namespace fileAPI.AsyncServices
{
    public interface IRabbitMQClient<T>
    {
        void sendMessage(T msg);
       
    }
}
