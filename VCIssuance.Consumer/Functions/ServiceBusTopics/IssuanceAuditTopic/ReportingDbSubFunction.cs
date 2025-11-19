using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VCIssuance.Core.Constants;

namespace VCIssuance.Consumer.Functions.ServiceBusTopics.IssuanceAuditTopic;

public class ReportingDbSubFunction(ILogger<ReportingDbSubFunction> logger)
{
    private readonly ILogger<ReportingDbSubFunction> _logger = logger;

    [Function(nameof(ReportingDbSubFunction))]
    public void Run(
        [ServiceBusTrigger(
            TopicSubscriptionNames.IssuanceAuditTopic.TopicName,
            TopicSubscriptionNames.IssuanceAuditTopic.ReportingDBSub,
            Connection = ConnectionNames.AzureServiceBus)]
        ServiceBusReceivedMessage messageReceived) 
        => this._logger.LogInformation("{FunctionName}: ReportingDbSub subscription received message: '{MessageId}'", 
            nameof(ReportingDbSubFunction), 
            messageReceived.MessageId);
}