namespace Application.Dtos.Order.OrderItem
{
    /// <summary>
    /// مدل نمایش آیتم سفارش برای مدیر
    /// </summary>
    public class AdminOrderItemDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string? UserName { get; set; } = string.Empty;
        /// <summary>
        /// نام محصول
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// قیمت محصول
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// قیمت کل
        /// </summary>
        public int TotalPrice { get; set; }
        /// <summary>
        /// تعداد محصول
        /// </summary>
        public int Quantity { get; set; }
    }
}
