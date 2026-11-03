using System.Text.Json.Serialization;

namespace Domain.Common
{
    /// <summary>
    /// پاسخ دریافت شده از درگاه پس از ایجاد درخواست پرداخت
    /// </summary>
    public class ZarinpalRequestResponse
    {
        /// <summary>
        /// اطلاعات نتیجه درخواست پرداخت
        /// </summary>
        [JsonPropertyName("data")]
        public RequestData Data { get; set; } = new();
    }

    /// <summary>
    /// اطلاعات نتیجه ایجاد درخواست پرداخت
    /// </summary>
    public class RequestData
    {
        /// <summary>
        /// کد نتیجه درخواست درگاه
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// پیام برگشتی درگاه
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// شناسه پرداخت ایجاد شده توسط درگاه
        /// </summary>
        [JsonPropertyName("authority")]
        public string? Authority { get; set; }
    }
}
