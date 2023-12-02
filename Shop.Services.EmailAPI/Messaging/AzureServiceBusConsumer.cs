using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Shop.Services.EmailAPI.Models.Dto;
using Shop.Services.EmailAPI.Service;
using System.Text;

namespace Shop.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBustConnectionString;
        private readonly string emailCartQueue;
        private readonly string newUserQueue;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _newUserProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            serviceBustConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            newUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:NewUserRegisteredQueue");

            var client = new ServiceBusClient(serviceBustConnectionString);

            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _newUserProcessor = client.CreateProcessor(newUserQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            _newUserProcessor.ProcessMessageAsync += OnNewUserRegistered;
            _newUserProcessor.ProcessErrorAsync += ErrorHandler;


            await _emailCartProcessor.StartProcessingAsync();
            await _newUserProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _newUserProcessor.StopProcessingAsync();
            await _newUserProcessor.DisposeAsync();
        }

        private async Task OnNewUserRegistered(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            string objMessage = JsonConvert.DeserializeObject<string>(body);

            try
            {
                await _emailService.NewUserLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
