using Application.Interfaces.Services;
using Domain.Common;
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
        public ZarinpalPaymentGateway(HttpClient httpClient, IOptions<ZarinpalOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }
        #endregion

        #region Request
        public async Task<PaymentRequestResult> RequestAsync(int amount, int orderId, string callbackUrl)
        {
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
                    return new(false, null, null, ".خطا در ارتباط با درگاه");
                }

                var result = await response.Content.ReadFromJsonAsync<ZarinpalRequestResponse>();

                if (result?.Data.Code != 100)
                {
                    return new(false, null, null, ".درخواست پرداخت ناموفق بود");
                }

                var authority = result.Data.Authority;

                return new(true, authority, _options.Value.StartPayUrl, null);
            }
            catch (TaskCanceledException)
            {
                return new(false, null, null, ".زمان پاسخگویی درگاه به پایان رسید");
            }
            catch (HttpRequestException)
            {
                return new(false, null, null, ".ارتباط با درگاه برقرار نشد");
            }
            catch (JsonException)
            {
                return new(false, null, null, ".پاسخ دریافتی از درگاه معتبر نیست");
            }

        }
        #endregion

        #region Verify
        public async Task<PaymentVerificationResult> VerifyAsync(int amount, string authority)
        {
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
                    return new(false, null, ".خطا در ارتباط با درگاه");
                }

                var result = await response.Content.ReadFromJsonAsync<ZarinpalVerifyResponse>();

                if (result?.Data.Code is not (100 or 101))
                {
                    return new(false, null, ".پرداخت تأیید نشد");
                }

                return new(true, result.Data.RefId.ToString(), null);
            }
            catch (TaskCanceledException)
            {
                return new(false, null, ".زمان پاسخگویی درگاه به پایان رسید");
            }
            catch (HttpRequestException)
            {
                return new(false, null, ".ارتباط با درگاه برقرار نشد");
            }
            catch (JsonException)
            {
                return new(false, null, ".پاسخ دریافتی از درگاه معتبر نیست");
            }
        }
        #endregion
    }
}
