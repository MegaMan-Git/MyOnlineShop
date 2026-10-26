using Application.Interfaces.Services;
using Domain.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ZarinpalPaymentGateway : IPaymentGateway
    {
        #region DI
        private readonly HttpClient _httpClient;
        private readonly IOptions<ZarinpalOptions> _options;
        private readonly ILogger<ZarinpalPaymentGateway> _logger;

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
