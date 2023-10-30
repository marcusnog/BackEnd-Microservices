using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MsProductIntegration.Contracts.Services;
using MsProductIntegration.Contracts.UseCases;

namespace MsProductIntegration.Services
{
    internal class ProductQueueProcessor : QueueWorker
    {
        IProcessProductQueueMessageUseCase _processProductQueueMessageUseCase;
        public ProductQueueProcessor(IConfiguration configuration, IProcessProductQueueMessageUseCase processProductQueueMessageUseCase, ILogger<ProductQueueProcessor> logger, IServiceBusClientFactory serviceBusClientFactory)
            : base(configuration, logger, serviceBusClientFactory)
        {
            _processProductQueueMessageUseCase = processProductQueueMessageUseCase;
        }

        protected override async Task ProcessMessage(string rawMessageBody, string messageId, IReadOnlyDictionary<string, object> userProperties, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Processing message {messageId}", messageId);

            await _processProductQueueMessageUseCase.Process(rawMessageBody);

            Logger.LogInformation("Message {messageId} processed", messageId);
        }
    }
}
