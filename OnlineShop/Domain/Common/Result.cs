namespace Domain.Common
{
    /// <summary>
    /// مدل پاسخ لایه ریپازیتوری
    /// </summary>
    public class Result
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        
        /// <summary>
        /// نقش کاربر
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// نتیجه عملیات
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// فهرست خطاها
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}
