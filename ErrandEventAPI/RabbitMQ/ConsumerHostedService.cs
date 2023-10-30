using ErrandEventAPI.Controllers;
using ErrandEventAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ErrandEventAPI.RabbitMQ
{
    public class ConsumerHostedService: BackgroundService
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        readonly ILogger<ConsumerHostedService> _logger;
        IServiceProvider _serviceProvider;
        public ConsumerHostedService (ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            this.factory = new ConnectionFactory() { HostName = "localhost" };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            channel.QueueDeclare("user", exclusive: false);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            await Task.Run(Listen, stoppingToken);
        }
        private async Task Listen()
        {
            while (true)
            {
                bool autoAck = false;
                BasicGetResult result = channel.BasicGet("user", autoAck);
                if (result == null)
                {

                }
                else
                {
                    IBasicProperties basicProperties = result.BasicProperties;
                    var body = result.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation(message);
                    channel.BasicAck(result.DeliveryTag, false);
                }
            }
/*
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, EventArgs) =>
            {
                var body = EventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                HandleMessage(message);
                channel.BasicAck(EventArgs.DeliveryTag, false);
                _logger.LogInformation(message);

            };
            channel.BasicConsume(queue: "user", autoAck: true, consumer: consumer);*/
            await Task.CompletedTask;
        }
        private void HandleMessage(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _eventService = scope.ServiceProvider.GetService<IEventService>();
                UserMessage userMessage = JsonConvert.DeserializeObject<UserMessage>(message);

                if (userMessage.Action == MESSAGE_ACTION.ADD)
                {
                    _eventService.AddUserToEvent(userMessage);
                }
                else if (userMessage.Action == MESSAGE_ACTION.REMOVE)
                {
                    _eventService.RemoveUserFromEvent(userMessage);
                }
            }
        }

        public override void Dispose()
        {
            /*channel.Close();
            connection.Close();
            base.Dispose();*/
        }
    }
}
