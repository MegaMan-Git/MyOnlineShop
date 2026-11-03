namespace Domain.Common
{
    /// <summary>
    /// تنظیمات مورد نیاز برای ارتباط با درگاه زرین پال
    /// </summary>
    public class ZarinpalOptions
    {
        /// <summary>
        /// شناسه پذیرنده درگاه
        /// </summary>
        public string MerchantId { get; set; } = string.Empty;
        
        /// <summary>
        /// نشانی ایجاد درخواست پرداخت
        /// </summary>
        public string RequestUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// نشانی بررسی و تایید پرداخت
        /// </summary>
        public string VerifyUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// نشانی ورود به صفحه پرداخت
        /// </summary>
        public string StartPayUrl { get; set; } = string.Empty;
    }
}
