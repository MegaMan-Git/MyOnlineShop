using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Common
{
    public class ZarinpalRequestResponse
    {
        [JsonPropertyName("data")]
        public RequestData Data { get; set; } = new();
    }

    public class RequestData
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("authority")]
        public string? Authority { get; set; }     
    }
}
