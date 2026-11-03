namespace Application.Dtos.Cart.Cartitem
{
    /// <summary>
    /// مدل نمایش آیتم سبد خرید برای مدیر
    /// </summary>
    public class AdminCartItemDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// شناسه سبد خرید
        /// </summary>
        public int CartId { get; set; }
        /// <summary>
        /// شناسه محصول
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// نام محصول
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string? UserName { get; set; } = string.Empty;
        /// <summary>
        /// تعداد محصول
        /// </summary>
        public int Quantity { get; set; }
    }
}
