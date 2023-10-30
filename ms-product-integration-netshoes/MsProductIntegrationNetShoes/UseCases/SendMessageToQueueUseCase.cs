using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using MsProductIntegrationNetShoes.Contracts.UseCases;
using Newtonsoft.Json;
namespace MsProductIntegrationNetShoes.UseCases
{
    public class SendMessageToQueueUseCase : ISendMessageToQueueUseCase
    {
        readonly IConfiguration _configuration;

        readonly string _azureQueue;
        readonly string _azureQueueConnectionString;

        public SendMessageToQueueUseCase(IConfiguration configuration)
        {
            _configuration = configuration;
            _azureQueue = _configuration.GetValue<string>("AZURE_QUEUE");
            _azureQueueConnectionString = _configuration.GetValue<string>("AZURE_QUEUE_CONNECTION");
        }

        public async Task Queue(object obj)
        {
            await Queue(obj, _azureQueue, _azureQueueConnectionString);
        }
        public async Task Queue(object obj, string queueName, string queueConnection)
        {
            var serviceBusClient = new ServiceBusClient(queueConnection);
            var serviceBusSender = serviceBusClient.CreateSender(queueName);

            var message = new ServiceBusMessage(JsonConvert.SerializeObject(obj));

            await serviceBusSender.SendMessageAsync(message);
        }
    }
}
