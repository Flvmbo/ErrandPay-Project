using ErrandEventAPI.Models;

namespace ErrandEventAPI.RabbitMQ
{
    public interface IEventProducer    {
        public void SendUserMessage <T> (T message);
    }
}
