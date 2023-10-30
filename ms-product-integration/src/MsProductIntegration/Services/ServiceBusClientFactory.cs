using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
//using Azure.Identity;
using Microsoft.Extensions.Logging;
using MsProductIntegration.Contracts.Services;

namespace MsProductIntegration.Services
{
    public class ServiceBusClientFactory : IServiceBusClientFactory
    {
        readonly IConfiguration _configuration;
        readonly ILogger<ServiceBusClientFactory> _logger;
        public ServiceBusClientFactory(IConfiguration configuration, ILogger<ServiceBusClientFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        //public static ServiceBusClient CreateWithManagedIdentityAuthentication(IConfiguration configuration, ILogger logger)
        //{
        //    var hostname = configuration.GetValue<string>("KEDA_SERVICEBUS_HOST_NAME");

        //    var clientIdentityId = configuration.GetValue<string>("KEDA_SERVICEBUS_IDENTITY_USERASSIGNEDID", defaultValue: null);
        //    if (string.IsNullOrWhiteSpace(clientIdentityId) == false)
        //    {
        //        logger.LogInformation("Using user-assigned identity with ID {UserAssignedIdentityId}", clientIdentityId);
        //    }

        //    return new ServiceBusClient(hostname, new ManagedIdentityCredential(clientId: clientIdentityId));
        //}

        //public static ServiceBusClient CreateWithServicePrincipleAuthentication(IConfiguration configuration)
        //{
        //    var hostname = configuration.GetValue<string>("KEDA_SERVICEBUS_HOST_NAME");
        //    var tenantId = configuration.GetValue<string>("KEDA_SERVICEBUS_TENANT_ID");
        //    var appIdentityId = configuration.GetValue<string>("KEDA_SERVICEBUS_IDENTITY_APPID");
        //    var appIdentitySecret = configuration.GetValue<string>("KEDA_SERVICEBUS_IDENTITY_SECRET");

        //    return new ServiceBusClient(hostname, new ClientSecretCredential(tenantId, appIdentityId, appIdentitySecret));
        //}

        public ServiceBusClient CreateWithConnectionStringAuthentication()
        {
            var connectionString = _configuration.GetValue<string>("KEDA_SERVICEBUS_QUEUE_CONNECTIONSTRING");
            return new ServiceBusClient(connectionString);
        }
    }
}
