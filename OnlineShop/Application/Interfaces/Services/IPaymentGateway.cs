using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface IPaymentGateway
    {
        /// <summary>
        /// را برمیگرداند Authority برای سفارش، درخواست پرداخت ایجاد میکند و در صورت موفقیت
        /// </summary>
        /// <param name="amount">مبلغ قابل پرداخت سفارش</param>
        /// <param name="orderId">شناسه سفارش مربوط به پرداخت</param>
        /// <param name="callbackUrl">
        /// لینکی که درگاه پرداخت پس از اتمام پرداخت، کلاینت را به آن هدایت میکند
        /// </param>
        /// <returns>و نشانی شروع پرداخت یا پیام خطا Authority نتیجه درخواست شامل</returns>
        Task<PaymentRequestResult> RequestAsync(int amount, int orderId, string callbackUrl);

        /// <summary>
        /// ارسال شده از زرین پال بررسی میکند Authority وضعیت پرداخت را با استفاده از
        /// </summary>
        /// <param name="amount">مبلغ پرداخت</param>
        /// <param name="authority">شناسه پرداخت دریافت شده از درگاه</param>
        /// <returns>در صورت موفقیت یا پیام خطا ReferenceId نتیجه بررسی شامل</returns>
        Task<PaymentVerificationResult> VerifyAsync(int amount, string authority);
    }
}
