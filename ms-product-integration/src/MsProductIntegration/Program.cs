using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsProductIntegration.Contracts.Data;
using MsProductIntegration.Contracts.Repositories;
using MsProductIntegration.Contracts.Services;
using MsProductIntegration.Contracts.UseCases;
using MsProductIntegration.Data;
using MsProductIntegration.Repositories;
using MsProductIntegration.Services;
using MsProductIntegration.UseCases;

namespace Keda.Samples.Dotnet.OrderProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
                {
                    loggingBuilder.AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss] ");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ICatalogContext, CatalogContext>();
                    services.AddSingleton<IProductRepository, ProductRepository>();
                    services.AddSingleton<IProductSkuBillRepository, ProductSkuBillRepository>();
                    services.AddSingleton<IProductSkuPreDefinedPriceRepository, ProductSkuPreDefinedPriceRepository>();
                    services.AddSingleton<IProductSkuRangePriceRepository, ProductSkuRangePriceRepository>();
                    services.AddSingleton<ICategoryRepository, CategoryRepository>();
                    services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>();
                    services.AddTransient<IProcessProductQueueMessageUseCase, ProcessProductQueueMessageUseCase>();
                    services.AddHostedService<ProductQueueProcessor>();
                });
    }
}