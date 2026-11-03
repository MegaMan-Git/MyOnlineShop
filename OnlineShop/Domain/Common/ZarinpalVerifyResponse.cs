using System.Text.Json.Serialization;

namespace Domain.Common
{
    /// <summary>
    /// پاسخ دریافت شده از درگاه پس از بررسی پرداخت
    /// </summary>
    public class ZarinpalVerifyResponse
    {
        /// <summary>
        /// اطلاعات نتیجه بررسی پرداخت
        /// </summary>
        [JsonPropertyName("data")]
        public VerifyData Data { get; set; } = new();
    }

    /// <summary>
    /// اطلاعات نتیجه بررسی پرداخت
    /// </summary>
    public class VerifyData
    {
        /// <summary>
        /// کد نتیجه بررسی درگاه
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// شناسه پیگیری پرداخت
        /// </summary>
        [JsonPropertyName("ref_id")]
        public int RefId { get; set; }
    }
}
