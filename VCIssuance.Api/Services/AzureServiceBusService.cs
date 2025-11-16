using Azure.Messaging.ServiceBus;
using System.Text.Json;
using VCIssuance.Core.Interfaces;
using VCIssuance.Core.Models;

namespace VCIssuance.Api.Services;

public class AzureServiceBusService(ServiceBusClient serviceBusClient) : IMessageService
{
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;

    public async Task SendMessageAsync(string queueName, IssuanceRequestModel payload)
    {
        // Create a sender for the target queue
        var sender = _serviceBusClient.CreateSender(queueName);

        var messageBody = JsonSerializer.Serialize(payload);

        var message = new ServiceBusMessage(messageBody);

        await sender.SendMessageAsync(message);
    }
}
