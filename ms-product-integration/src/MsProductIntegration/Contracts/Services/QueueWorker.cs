using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MsProductIntegration.Contracts.Enum;
using MsProductIntegration.Services;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging;
using System;
using System.Collections.Generic;
using MsProductIntegration.Contracts.Exceptions;

namespace MsProductIntegration.Contracts.Services
{
    public abstract class QueueWorker : BackgroundService
    {
        protected ILogger<QueueWorker> Logger { get; }
        protected IConfiguration Configuration { get; }
        protected IServiceBusClientFactory ServiceBusClientFactory { get; }
        protected QueueWorker(IConfiguration configuration, ILogger<QueueWorker> logger, IServiceBusClientFactory serviceBusClientFactory)
        {
            Configuration = configuration;
            Logger = logger;
            ServiceBusClientFactory = serviceBusClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = Configuration.GetValue<string>("KEDA_SERVICEBUS_QUEUE_NAME");
            var messageProcessor = CreateServiceBusProcessor(queueName);
            messageProcessor.ProcessMessageAsync += HandleMessageAsync;
            messageProcessor.ProcessErrorAsync += HandleReceivedExceptionAsync;

            Logger.LogInformation($"Starting message pump on queue {queueName} in namespace {messageProcessor.FullyQualifiedNamespace}");
            await messageProcessor.StartProcessingAsync(stoppingToken);
            Logger.LogInformation("Message pump started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Logger.LogInformation("Closing message pump");
            await messageProcessor.CloseAsync(cancellationToken: stoppingToken);
            Logger.LogInformation("Message pump closed : {Time}", DateTimeOffset.UtcNow);
        }

        private ServiceBusProcessor CreateServiceBusProcessor(string queueName)
        {
            var serviceBusClient = AuthenticateToAzureServiceBus();
            var messageProcessor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions() { AutoCompleteMessages = false });
            return messageProcessor;
        }

        private ServiceBusClient AuthenticateToAzureServiceBus()
        {
            var authenticationMode = AuthenticationMode.ConnectionString;// Configuration.GetValue<AuthenticationMode>("KEDA_SERVICEBUS_AUTH_MODE");

            ServiceBusClient serviceBusClient;

            switch (authenticationMode)
            {
                case AuthenticationMode.ConnectionString:
                    Logger.LogInformation($"Authentication by using connection string");
                    serviceBusClient = ServiceBusClientFactory.CreateWithConnectionStringAuthentication();
                    break;
                //case AuthenticationMode.ServicePrinciple:
                //    Logger.LogInformation("Authentication by using service principle");
                //    serviceBusClient = ServiceBusClientFactory.CreateWithServicePrincipleAuthentication(Configuration);
                //    break;
                //case AuthenticationMode.ManagedIdentity:
                //    Logger.LogInformation("Authentication by using managed identity");
                //    serviceBusClient = ServiceBusClientFactory.CreateWithManagedIdentityAuthentication(Configuration, Logger);
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return serviceBusClient;
        }

        private async Task HandleMessageAsync(ProcessMessageEventArgs processMessageEventArgs)
        {
            try
            {
                var rawMessageBody = Encoding.UTF8.GetString(processMessageEventArgs.Message.Body.ToArray());
                Logger.LogInformation("Received message {MessageId} with body {MessageBody}",
                    processMessageEventArgs.Message.MessageId, rawMessageBody);


                await ProcessMessage(rawMessageBody, processMessageEventArgs.Message.MessageId,
                    processMessageEventArgs.Message.ApplicationProperties,
                    processMessageEventArgs.CancellationToken);

                Logger.LogInformation("Message {MessageId} processed", processMessageEventArgs.Message.MessageId);

                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
            }
            catch (InvalidEventOperationException ex)
            {
                Logger.LogError(ex, "Unable to handle message");
                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to handle message");
            }
        }

        private Task HandleReceivedExceptionAsync(ProcessErrorEventArgs exceptionEvent)
        {
            Logger.LogError(exceptionEvent.Exception, "Unable to process message");
            return Task.CompletedTask;
        }

        protected abstract Task ProcessMessage(string rawMessageBody, string messageId, IReadOnlyDictionary<string, object> userProperties, CancellationToken cancellationToken);
    }
}
