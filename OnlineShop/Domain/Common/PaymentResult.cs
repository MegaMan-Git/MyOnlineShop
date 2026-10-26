namespace Domain.Common
{
    public record PaymentRequestResult(
        bool IsSuccessful,
        string? Authority,
        string? PaymentUrl,
        string? ErrorMessage);

    public record PaymentVerificationResult(
        bool IsSuccessful,
        string? ReferenceId,
        string? ErrorMessage);
}
