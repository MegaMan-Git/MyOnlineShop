namespace Application.Dtos.Cart.Cartitem
{
    /// <summary>
    /// مدل افزودن محصول به سبد خرید
    /// </summary>
    public class AddCartItemDto
    {
        /// <summary>
        /// شناسه محصول
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// تعداد محصول
        /// </summary>
        public int Quantity { get; set; }
    }
}
