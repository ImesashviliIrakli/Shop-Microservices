
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Service.RewardAPI.Message;
using Shop.Services.RewardAPI.Service;
using System.Text;

namespace Shop.Services.RewardAPI.Messaging
{
    public class RabbitMqOrderConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;

        private IConnection _connection;
        private IModel _channel;

        private readonly string orderCreatedExchange;
        string queueName = string.Empty;
        public RabbitMqOrderConsumer(IConfiguration configuration, RewardService emailService)
        {
            _configuration = configuration;
            _rewardService = emailService;

            orderCreatedExchange = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(orderCreatedExchange, ExchangeType.Fanout);

            queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queueName, orderCreatedExchange, "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                RewardsMessage rewardsMessage = JsonConvert.DeserializeObject<RewardsMessage>(content);

                HandleMessaage(rewardsMessage).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessaage(RewardsMessage rewardsMessage)
        {
            await _rewardService.UpdateRewards(rewardsMessage);
        }
    }
}
