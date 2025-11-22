using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VCIssuance.Core.Constants;

namespace VCIssuance.DLQListener.Functions;

public class RevalidationSchedulerDLQFunction(ILogger<RevalidationSchedulerDLQFunction> logger)
{
    private readonly ILogger<RevalidationSchedulerDLQFunction> _logger = logger;

    [Function(nameof(RevalidationSchedulerDLQFunction))]
    public async Task Run(
        [ServiceBusTrigger(QueueNames.RevalidationRequestsDLQ, Connection = ConnectionNames.AzureServiceBus)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        this._logger.LogInformation("{FunctionName}: RevalidationSchedulerDLQFunction triggered for message with ID: {MessageId}", nameof(RevalidationSchedulerDLQFunction), message.MessageId);
        this._logger.LogInformation("{FunctionName}: Message Body: {MessageBody}", nameof(RevalidationSchedulerDLQFunction), message.Body.ToString());
        this._logger.LogInformation("{FunctionName}: Dead-letter Reason: {DeadLetterReason}", nameof(RevalidationSchedulerDLQFunction), message.DeadLetterReason);
        this._logger.LogInformation("{FunctionName}: Dead-letter Error Description: {DeadLetterErrorDescription}", nameof(RevalidationSchedulerDLQFunction), message.DeadLetterErrorDescription);

        await messageActions.CompleteMessageAsync(message);
        this._logger.LogInformation("{FunctionName}: Message with ID: {MessageId} has been processed and removed from DLQ.", nameof(RevalidationSchedulerDLQFunction), message.MessageId);
    }
}