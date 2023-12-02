using Shop.Services.EmailAPI.Messaging;
using System.Reflection.Metadata;

namespace Shop.Services.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
        public static IApplicationBuilder UserAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();

            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopped.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }

        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }
    }
}
