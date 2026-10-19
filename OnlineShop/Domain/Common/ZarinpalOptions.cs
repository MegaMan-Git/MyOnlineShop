using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class ZarinpalOptions
    {
        public string MerchantId { get; set; } = string.Empty;
        public string RequestUrl { get; set; } = string.Empty;
        public string VerifyUrl { get; set; } = string.Empty;
        public string StartPayUrl { get; set; } = string.Empty;
    }
}
