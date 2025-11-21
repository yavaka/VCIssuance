using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VCIssuance.Consumer.Logic;
using VCIssuance.Core.Constants;
using VCIssuance.Core.Models;

namespace VCIssuance.Consumer.Functions.ServiceBusQueues;

public class IssuanceProcessorFunction(
    ILogger<IssuanceProcessorFunction> logger,
    ServiceBusClient serviceBusClient,
    VCIssuanceWorkflow workflow)
{
    private readonly ILogger<IssuanceProcessorFunction> _logger = logger;
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;
    private readonly VCIssuanceWorkflow _workflow = workflow;

    [Function(nameof(IssuanceProcessorFunction))]
    public async Task Run(
        [ServiceBusTrigger(
            QueueNames.IssuanceRequests, 
            Connection = ConnectionNames.AzureServiceBus, 
            IsSessionsEnabled = true)] // FIFO processing per user
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions actions)
    {
        if (message == null)
        {
            this._logger.LogError("{FunctionName}: Received null message.", nameof(IssuanceProcessorFunction));
            return;
        }

        this._logger.LogInformation("{FunctionName}: ServiceBus queue trigger function processing message: '{MessageId}'", nameof(IssuanceProcessorFunction), message.MessageId);

        try
        {
            var request = JsonSerializer.Deserialize<IssuanceRequestModel>(message.Body);

            if (request == null)
            {
                this._logger.LogError("{FunctionName}: Failed to deserialize message body: '{MessageId}'", nameof(IssuanceProcessorFunction), message.MessageId);
                throw new InvalidOperationException("Message body is null or invalid.");
            }

            await _workflow.ExecuteIssuanceAsync(request);

            // Send audit message to IssuanceAuditTopic
            var sender = _serviceBusClient.CreateSender(TopicSubscriptionNames.IssuanceAuditTopic.TopicName);
            var auditMessage = new ServiceBusMessage(message.Body)
            {
                Subject = "VCCreated",
                CorrelationId = message.MessageId,
                ApplicationProperties =
                {
                    { "vc_type", request.VCType },
                }
            };
            await sender.SendMessageAsync(auditMessage);
            
            await actions.CompleteMessageAsync(message);

            this._logger.LogInformation("{FunctionName}: ServiceBus queue trigger function processed message: '{MessageId}'", nameof(IssuanceProcessorFunction), message.MessageId);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "{FunctionName}: Error processing VC Issuance request. '{MessageId}'", nameof(IssuanceProcessorFunction), message.MessageId);
            await actions.AbandonMessageAsync(message);
        }
    }
}