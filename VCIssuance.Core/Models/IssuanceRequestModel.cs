namespace VCIssuance.Core.Models;

public class IssuanceRequestModel
{
    /// <summary>
    /// The unique identifier for the user 
    /// </summary>
    public required string RecipientIdentifier { get; set; }

    /// <summary>
    /// The type of credential being requested (Employee, Financial, etc.)
    /// </summary>
    public required string VCType { get; set; }

    /// <summary>
    /// Optional: Any specific data needed for the VC content
    /// </summary>
    public Dictionary<string, object> ClaimData { get; set; } = [];
}
