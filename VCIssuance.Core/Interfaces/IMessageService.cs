using VCIssuance.Core.Models;

namespace VCIssuance.Core.Interfaces;

public interface IMessageService
{
    /// <summary>
    /// Method to send a message payload to a specified queue
    /// </summary>
    Task SendMessageAsync(string queueName, IssuanceRequestModel payload);
}
