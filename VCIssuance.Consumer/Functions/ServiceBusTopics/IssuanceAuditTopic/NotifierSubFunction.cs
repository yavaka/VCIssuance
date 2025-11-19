using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VCIssuance.Core.Constants;

namespace VCIssuance.Consumer.Functions.ServiceBusTopics.IssuanceAuditTopic;

public class NotifierSubFunction(ILogger<NotifierSubFunction> logger)
{
    private readonly ILogger<NotifierSubFunction> _logger = logger;

    [Function(nameof(NotifierSubFunction))]
    public void Run(
        [ServiceBusTrigger(
            TopicSubscriptionNames.IssuanceAuditTopic.TopicName, 
            TopicSubscriptionNames.IssuanceAuditTopic.NotifierSub, 
            Connection = ConnectionNames.AzureServiceBus)]
        ServiceBusReceivedMessage messageReceived) 
        => this._logger.LogInformation("{FunctionName}: NotifierSub subscription received message: '{MessageId}'",
            nameof(NotifierSubFunction),
            messageReceived.MessageId);
}