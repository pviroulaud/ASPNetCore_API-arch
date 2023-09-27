using fileDTO;
using logDTO;

namespace documentAPI.AsyncServices
{
    public interface IRabbitMQClient<T>
    {
        void sendMessage(T msg);
       
    }
}
