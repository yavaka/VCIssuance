namespace VCIssuance.Core.Models;

public class RevalidationScheduleModel
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public required string RecipientIdentifier { get; set; }
    /// <summary>
    /// The type of credential being requested (Employee, Financial, etc.)
    /// </summary>
    public required string VCType { get; set; }
    /// <summary>
    /// Expiration date and time in (UTC).
    /// </summary>
    public required DateTime ExpirationDateUtc { get; set; }
}
