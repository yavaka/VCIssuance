using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VCIssuance.Core.Constants;

namespace VCIssuance.Consumer.Functions.ServiceBusTopics.IssuanceAuditTopic;

public class NotifierSubFunction(ILogger<NotifierSubFunction> logger)
{
    private readonly ILogger<NotifierSubFunction> _logger = logger;

    [Function(nameof(NotifierSubFunction))]
    public async Task Run(
        [ServiceBusTrigger(
            TopicSubscriptionNames.IssuanceAuditTopic.TopicName,
            TopicSubscriptionNames.IssuanceAuditTopic.NotifierSub,
            Connection = ConnectionNames.AzureServiceBus)]
        ServiceBusReceivedMessage messageReceived,
        ServiceBusMessageActions actions)
    {
        // If the message matches the CriticalNotifierFilter, process it accordingly
        if (messageReceived.ApplicationProperties[MessageFilters.CriticalNotifierFilter.Key].ToString() == MessageFilters.CriticalNotifierFilter.Value)
        {
            this._logger.LogInformation("{FunctionName}: NotifierSub subscription received message: '{MessageId}'",
                nameof(NotifierSubFunction),
                messageReceived.MessageId);
        }
        await actions.CompleteMessageAsync(messageReceived);
    }
}