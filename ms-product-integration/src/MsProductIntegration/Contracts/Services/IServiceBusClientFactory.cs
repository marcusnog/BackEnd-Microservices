using Azure.Messaging.ServiceBus;

namespace MsProductIntegration.Contracts.Services
{
    public interface IServiceBusClientFactory
    {
        ServiceBusClient CreateWithConnectionStringAuthentication();
    }
}
