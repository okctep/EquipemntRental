using Application.Configuration;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Application
{
    class Program
    {
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider.GetService<RpcServer>().Start();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            IConfigurationRoot configuration;

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.Configure<RpcServerConfiguration>(configuration.GetSection("RpcServerOptions"));

            serviceProvider = serviceCollection
                .AddLogging()
                .AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<RpcServer>>())
                .AddSingleton<RpcServer>()
                .AddSingleton<RentalPriceCalculationService>()
                .BuildServiceProvider();

            serviceCollection.AddSingleton<IServiceProvider>(serviceProvider);
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
        }
    }
}
