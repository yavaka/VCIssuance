namespace VCIssuance.Core.Constants;

public static class QueueNames
{
    public const string IssuanceRequests = "vc-issuance-requests-queue";
    public const string IssuanceRequestsDLQ = "vc-issuance-requests-queue/$DeadLetterQueue";
    public const string RevalidationRequests = "vc-revalidation-queue";
    public const string RevalidationRequestsDLQ = "vc-revalidation-queue/$DeadLetterQueue";
}
