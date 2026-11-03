namespace Domain.Common
{
    /// <summary>
    /// مدل مخصوص برگرداندن نتیجه درخواست ایجاد پرداخت از درگاه
    /// </summary>
    /// <param name="IsSuccessful">در صورت موفقیت آمیز بودن true</param>
    /// <param name="Authority">شناسه تراکنش ایجاد شده توسط درگاه</param>
    /// <param name="PaymentUrl">آدرس پایه درگاه پرداخت</param>
    /// <param name="ErrorMessage">پیام خطا در صورت شکست</param>
    public record PaymentRequestResult(
        bool IsSuccessful,
        string? Authority,
        string? PaymentUrl,
        string? ErrorMessage);

    /// <summary>
    /// مدل مخصوص برگرداندن نتیجه درخواست بررسی پرداخت از درگاه
    /// </summary>
    /// <param name="IsSuccessful">در صورت موفقیت آمیز بودن true</param>
    /// <param name="ReferenceId">شناسه رفرنس در صورت موفق بودن تراکنش</param>
    /// <param name="ErrorMessage">پیام خطا در صورت شکست</param>
    public record PaymentVerificationResult(
        bool IsSuccessful,
        string? ReferenceId,
        string? ErrorMessage);
}
