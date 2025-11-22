using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VCIssuance.Core.Constants;
using VCIssuance.Core.Models;

namespace VCIssuance.Consumer.Functions.ServiceBusQueues;

public class RevalidationSchedulerFunction(ILogger<RevalidationSchedulerFunction> logger)
{
    private readonly ILogger<RevalidationSchedulerFunction> _logger = logger;

    [Function(nameof(RevalidationSchedulerFunction))]
    public async Task Run(
        [ServiceBusTrigger(QueueNames.RevalidationRequests, Connection = ConnectionNames.AzureServiceBus)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            this._logger.LogInformation("{FunctionName}: Processing revalidation scheduling message: '{MessageId}'", nameof(RevalidationSchedulerFunction), message.MessageId);
            
            var model = message.Body.ToObjectFromJson<RevalidationScheduleModel>();
            if (model is null)
            {
                this._logger.LogError("{FunctionName}: message body is null", nameof(RevalidationSchedulerFunction));
                throw new InvalidOperationException($"{nameof(RevalidationSchedulerFunction)}: Message body is null or invalid.");
            }
            this._logger.LogInformation("{FunctionName}: VC with ID: '{VcId}' will expire at '{RevalidationTime}'", nameof(RevalidationSchedulerFunction), model.RecipientIdentifier, model.ExpirationDateUtc);

            await messageActions.CompleteMessageAsync(message);
        }
        catch (Exception e)
        {
            this._logger.LogCritical(e, "{FunctionName}: Error processing revalidation scheduling message: '{MessageId}'", nameof(RevalidationSchedulerFunction), message.MessageId);
            await messageActions.AbandonMessageAsync(message);
        }
    }
}