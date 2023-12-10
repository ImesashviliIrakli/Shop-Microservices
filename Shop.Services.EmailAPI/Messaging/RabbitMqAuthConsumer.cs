
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Services.EmailAPI.Service;
using System.Text;

namespace Shop.Services.EmailAPI.Messaging
{
    public class RabbitMqAuthConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private IConnection _connection;
        private IModel _channel;
        
        private readonly string newUserQueue;
        
        public RabbitMqAuthConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            newUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:NewUserRegisteredQueue");


            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(newUserQueue, false, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                string email = JsonConvert.DeserializeObject<string>(content);
            
                HandleMessaage(email).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(newUserQueue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessaage(string email)
        {
            await _emailService.NewUserLog(email);
        }
    }
}
