using ErrandUserAPI.Models;

namespace ErrandUserAPI.RabbitMQ
{
    public interface IUserProducer
    {
        public void SendUserMessage <T> (T message);
    }
}
