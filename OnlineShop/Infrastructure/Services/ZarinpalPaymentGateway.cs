using Application.Interfaces.Services;
using Domain.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Services
{
    /// <summary>
    /// مسئول ارسال و تأیید درخواست های پرداخت از طریق درگاه زرین پال است
    /// </summary>
    public class ZarinpalPaymentGateway : IPaymentGateway
    {
        #region DI
        private readonly HttpClient _httpClient;
        private readonly IOptions<ZarinpalOptions> _options;
        private readonly ILogger<ZarinpalPaymentGateway> _logger;

        /// <summary>
        /// وابستگی های موردنیاز درگاه پرداخت را از طریق تزریق وابستگی دریافت میکند
        /// </summary>
        /// <param name="httpClient">به درگاه استفاده میشود HTTP برای ارسال درخواست های</param>
        /// <param name="options">تنظیمات مربوط به درگاه زرین پال را در اختیار کلاس قرار میدهد</param>
        /// <param name="logger">برای ثبت رویدادها و خطاهای مربوط به پرداخت استفاده میشود</param>
        public ZarinpalPaymentGateway(
            HttpClient httpClient,
            IOptions<ZarinpalOptions> options,
            ILogger<ZarinpalPaymentGateway> logger)
        {
            _httpClient = httpClient;
            _options = options;
            _logger = logger;
        }
        #endregion

        #region Request
        /// <summary>
        /// را برمیگرداند Authority برای سفارش، درخواست پرداخت ایجاد میکند و در صورت موفقیت
        /// </summary>
        /// <param name="amount">مبلغ قابل پرداخت سفارش</param>
        /// <param name="orderId">شناسه سفارش مربوط به پرداخت</param>
        /// <param name="callbackUrl">
        /// لینکی که درگاه پرداخت پس از اتمام پرداخت، کلاینت را به آن هدایت میکند
        /// </param>
        /// <returns>و نشانی شروع پرداخت یا پیام خطا Authority نتیجه درخواست شامل</returns>
        public async Task<PaymentRequestResult> RequestAsync
            (int amount, int orderId, string callbackUrl)
        {
            _logger.LogInformation(
                "Starting payment request for OrderId {OrderId} with Amount {Amount}",
                orderId,
                amount);

            var body = new
            {
                merchant_id = _options.Value.MerchantId,
                amount,
                description = $"سفارش با آیدی #{orderId}",
                callback_url = callbackUrl
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_options.Value.RequestUrl, body);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Payment request gateway returned HTTP {StatusCode} for OrderId {OrderId}",
                        (int)response.StatusCode,
                        orderId);

                    return new(false, null, null, ".خطا در ارتباط با درگاه");
                }

                var result = await response.Content.ReadFromJsonAsync<ZarinpalRequestResponse>();

                if (result?.Data.Code != 100)
                {
                    _logger.LogWarning(
                        "Payment request rejected for OrderId {OrderId} with Code {Code}",
                        orderId,
                        result?.Data.Code);

                    return new(false, null, null, ".درخواست پرداخت ناموفق بود");
                }

                var authority = result.Data.Authority;

                _logger.LogInformation(
                    "Payment authority received for OrderId {OrderId}",
                    orderId);

                return new(true, authority, _options.Value.StartPayUrl, null);
            }
            catch (TaskCanceledException exception)
            {
                _logger.LogWarning(
                    exception, "Payment request timed out for OrderId {OrderId}", orderId);

                return new(false, null, null, ".زمان پاسخگویی درگاه به پایان رسید");
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(
                    exception, "Payment gateway connection failed for OrderId {OrderId}", orderId);

                return new(false, null, null, ".ارتباط با درگاه برقرار نشد");
            }
            catch (JsonException exception)
            {
                _logger.LogError(
                    exception, "Invalid payment gateway response for OrderId {OrderId}", orderId);

                return new(false, null, null, ".پاسخ دریافتی از درگاه معتبر نیست");
            }

        }
        #endregion

        #region Verify
        /// <summary>
        /// ارسال شده از زرین پال بررسی میکند Authority وضعیت پرداخت را با استفاده از
        /// </summary>
        /// <param name="amount">مبلغ پرداخت</param>
        /// <param name="authority">شناسه پرداخت دریافت شده از درگاه</param>
        /// <returns>در صورت موفقیت یا پیام خطا ReferenceId نتیجه بررسی شامل</returns>
        public async Task<PaymentVerificationResult> VerifyAsync(int amount, string authority)
        {
            _logger.LogInformation("Starting payment verification request");

            var body = new
            {
                merchant_id = _options.Value.MerchantId,
                amount,
                authority
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_options.Value.VerifyUrl, body);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Payment verification gateway returned HTTP {StatusCode}",
                        (int)response.StatusCode);

                    return new(false, null, ".خطا در ارتباط با درگاه");
                }

                var result = await response.Content.ReadFromJsonAsync<ZarinpalVerifyResponse>();

                if (result?.Data.Code is not (100 or 101))
                {
                    _logger.LogWarning(
                        "Payment verification rejected with Code {Code}",
                        result?.Data.Code);

                    return new(false, null, ".پرداخت تأیید نشد");
                }

                return new(true, result.Data.RefId.ToString(), null);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Payment verification timed out");
                return new(false, null, ".زمان پاسخگویی درگاه به پایان رسید");
            }
            catch (HttpRequestException)
            {
                _logger.LogError("Payment verification connection failed");
                return new(false, null, ".ارتباط با درگاه برقرار نشد");
            }
            catch (JsonException exception)
            {
                _logger.LogError(exception, "Invalid payment verification response");
                return new(false, null, ".پاسخ دریافتی از درگاه معتبر نیست");
            }
        }
        #endregion
    }
}
