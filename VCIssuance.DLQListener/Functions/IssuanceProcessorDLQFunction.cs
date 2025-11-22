using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VCIssuance.Core.Constants;

namespace VCIssuance.DLQListener.Functions;

public class IssuanceProcessorDLQFunction(ILogger<IssuanceProcessorDLQFunction> logger)
{
    private readonly ILogger<IssuanceProcessorDLQFunction> _logger = logger;

    [Function(nameof(IssuanceProcessorDLQFunction))]
    public void Run(
        [ServiceBusTrigger(QueueNames.IssuanceRequestsDLQ, Connection = ConnectionNames.AzureServiceBus)]
        ServiceBusReceivedMessage message)
    {
        this._logger.LogCritical("{FunctionName}: DLQ Listener triggered for message: '{MessageId}'", nameof(IssuanceProcessorDLQFunction), message.MessageId);
        this._logger.LogCritical("{FunctionName}: DLQ Message Body: '{MessageBody}'", nameof(IssuanceProcessorDLQFunction), message.Body.ToString());
        this._logger.LogCritical("{FunctionName}: DLQ Message DeadLetterReason: '{DeadLetterReason}'", nameof(IssuanceProcessorDLQFunction), message.DeadLetterReason);
    }
}