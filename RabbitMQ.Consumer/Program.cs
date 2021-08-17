using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddJsonFile("appsettings.json", false, true).Build();
                })
                .ConfigureServices((context, service) =>
                {
                    var appSettingsConfig = context.Configuration.GetSection(nameof(ApplicationSettings));
                    
                    service.AddOptions();
                    service.Configure<ApplicationSettings>(appSettingsConfig);
                    service.AddSingleton(appSettingsConfig);
                    service.AddHostedService<ConsumerService>();
                });
    }
}
