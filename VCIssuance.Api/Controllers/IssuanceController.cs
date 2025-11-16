using Microsoft.AspNetCore.Mvc;
using VCIssuance.Core.Constants;
using VCIssuance.Core.Interfaces;
using VCIssuance.Core.Models;

namespace VCIssuance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuanceController(
        ILogger<IssuanceController> logger, 
        IMessageService messageService) : ControllerBase
    {
        private readonly ILogger<IssuanceController> _logger = logger;
        private readonly IMessageService _messageService = messageService;

        [HttpPost]
        public async Task<IActionResult> RequestIssuance([FromBody] IssuanceRequestModel requestModel)
        {
            // Validation
            if (string.IsNullOrEmpty(requestModel.RecipientIdentifier)
                || string.IsNullOrEmpty(requestModel.VCType))
            {
                return BadRequest("Recipient Identifier and VC Type are required.");
            }

            try
            {
                await _messageService.SendMessageAsync(
                    QueueNames.IssuanceRequests,
                    requestModel);

                return Accepted(new { Message = "VC Issuance request received and queued." });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error while queuing VC issuance request.");
                return StatusCode(500, "An error occurred while queuing the request.");
            }
        }
    }
}
