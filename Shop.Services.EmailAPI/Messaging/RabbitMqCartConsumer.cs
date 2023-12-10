
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Services.EmailAPI.Models.Dto;
using Shop.Services.EmailAPI.Service;
using System.Text;

namespace Shop.Services.EmailAPI.Messaging
{
    public class RabbitMqCartConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private IConnection _connection;
        private IModel _channel;

        private readonly string emailCartQueue;

        public RabbitMqCartConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(emailCartQueue, false, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(content);

                HandleMessaage(cartDto).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(emailCartQueue, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessaage(CartDto cartDto)
        {
            await _emailService.EmailCartAndLog(cartDto);
        }
    }
}
