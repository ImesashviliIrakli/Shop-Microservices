﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Shop.Services.ShoppingCartAPI.RabbitMQSender
{
    public class RabbitMQCartMessageSender : IRabbitMQCartessageSender
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;
        public RabbitMQCartMessageSender()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";
        }

        public void SendMessage(object message, string queueName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.QueueDeclare(queueName, false, false, false, null);

                string messageString = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageString);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     null,
                                     body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    Password = _password,
                    UserName = _userName
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if(_connection != null)
            {
                return true;
            }

            CreateConnection();
            return true;
        }
    }
}
