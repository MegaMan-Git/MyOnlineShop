namespace Application.Dtos.Cart
{
    /// <summary>
    /// مدل نمایش سبد خرید برای مدیر
    /// </summary>
    public class AdminCartDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string? UserName { get; set; } = string.Empty;
    }
}
