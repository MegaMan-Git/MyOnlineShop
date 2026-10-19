using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Common
{
    public class ZarinpalVerifyResponse
    {
        [JsonPropertyName("data")]
        public VerifyData Data { get; set; } = new();
    }

    public class VerifyData
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("ref_id")]
        public int RefId { get; set; }
    }
}
