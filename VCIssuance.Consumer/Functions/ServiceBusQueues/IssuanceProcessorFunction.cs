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
    VCIssuanceWorkflow workflow)
{
    private readonly ILogger<IssuanceProcessorFunction> _logger = logger;
    private readonly VCIssuanceWorkflow _workflow = workflow;

    [Function(nameof(IssuanceProcessorFunction))]
    public async Task Run(
        [ServiceBusTrigger(QueueNames.IssuanceRequests, Connection = "AzureServiceBus:ConnectionString")]
        string messageBody)
    {
        this._logger.LogInformation($"C# ServiceBus queue trigger function processed message: {messageBody}");

        try
        {
            var request = JsonSerializer.Deserialize<IssuanceRequestModel>(messageBody);

            if (request == null)
            {
                this._logger.LogError("Failed to deserialize message body.");
                return; // Fail message processing (will go to DLQ after retries)
            }

            await _workflow.ExecuteIssuanceAsync(request);

            // On successful completion, the message is automatically completed (deleted) 
            // by the Azure Functions host runtime when the function exits without an unhandled exception. 
            // This is the default behavior for simple ASB triggers.
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing VC Issuance request.");
            // Throwing the exception will cause the message to be abandoned (retried) 
            // by the Azure Functions host runtime.
            throw;
        }
    }
}