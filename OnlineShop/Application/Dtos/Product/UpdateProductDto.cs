namespace Application.Dtos.Product
{
    /// <summary>
    /// مدل به روزرسانی محصول
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// نام محصول
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// قیمت محصول
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// توضیحات محصول
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// نشانی تصویر محصول
        /// </summary>
        public string? ImageUrl { get; set; }
        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// موجودی محصول
        /// </summary>
        public int Stock { get; set; }
    }
}
