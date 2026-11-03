namespace Application.Dtos.Cart.Cartitem
{
    /// <summary>
    /// مدل نمایش آیتم سبد خرید برای مشتری
    /// </summary>
    public class CustomerCartItemDto
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
        /// تعداد محصول
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// قیمت محصول
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// قیمت کل
        /// </summary>
        public int TotalPrice { get; set; }
    }
}
