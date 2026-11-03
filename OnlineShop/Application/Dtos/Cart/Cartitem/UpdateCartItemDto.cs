namespace Application.Dtos.Cart.Cartitem
{
    /// <summary>
    /// مدل تغییر تعداد آیتم سبد خرید
    /// </summary>
    public class UpdateCartItemDto
    {
        /// <summary>
        /// شناسه آیتم سبد خرید
        /// </summary>
        public int CartItemId { get; set; }
        /// <summary>
        /// تعداد محصول
        /// </summary>
        public int Quantity { get; set; }
    }
}
