using Domain.Enums;

namespace Domain.Common
{
    /// <summary>
    /// مدلی عمومی برای نمایش نتیجه عملیات سرویس
    /// </summary>
    /// <typeparam name="T">نوع داده موجود در پاسخ را مشخص میکند</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// وضعیت نتیجه عملیات را مشخص میکند
        /// </summary>
        public ResultStatusCode StatusCode { get; set; }
        
        /// <summary>
        /// پیام مربوط به نتیجه عملیات را نگهداری میکند
        /// </summary>
        public string? Message { get; set; }
        
        /// <summary>
        /// فهرست خطاهای رخ داده در عملیات را نگهداری میکند
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
        
        /// <summary>
        /// داده نتیجه عملیات را نگهداری میکند
        /// </summary>
        public T? Data { get; set; }
    }
}
