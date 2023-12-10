namespace Shop.Services.ShoppingCartAPI.RabbitMQSender
{
    public interface IRabbitMQCartessageSender
    {
        void SendMessage(Object message, string queueName);
    }
}
