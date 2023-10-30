using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MsProductIntegrationGiffty.Contracts.Services;
using MsProductIntegrationGiffty.Contracts.UseCases;
using MsProductIntegrationGiffty.Services;
using MsProductIntegrationGiffty.UseCases;

static class Program
{
    static async Task Main(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Set up configuration sources.
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true);
        if (!string.IsNullOrEmpty(environment))
        {
            builder
                .AddJsonFile($"appsettings.{environment}.json", optional: false);
        }

        var _configuration = builder.Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(_configuration);
        services.AddTransient<ISendMessageToQueueUseCase, SendMessageToQueueUseCase>();
        services.AddTransient<IIntegrationService, IntegrationService>();
        services.AddTransient<IIntegrateProductsUseCase, IntegrateProductsUseCase>();
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddConfiguration(_configuration.GetSection("Logging"));
            logging.SetMinimumLevel(LogLevel.Information);
        });

        var provider = services.BuildServiceProvider();
        var integrateProductUseCase = provider.GetRequiredService<IIntegrateProductsUseCase>();

        var integrationtype = _configuration.GetValue<string>("INTEGRATION_TYPE");

        switch (integrationtype)
        {
            case "PRODUCTS":
                await integrateProductUseCase.IntegrateProducts();
                break;
            //case "PRICES":
            //    await integrateProductUseCase.IntegratePrices();
            //    break;
            case "AVAILABILITY":
                await integrateProductUseCase.IntegrateAvailability();
                break;
            case "STORES":
                await integrateProductUseCase.IntegrateStores();
                break;
            default:
                throw new Exception("Integration type is not defined");
                break;
        }
    }
}