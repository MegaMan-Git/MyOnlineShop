namespace Application.Dtos.Category
{
    /// <summary>
    /// مدل به روزرسانی دسته بندی
    /// </summary>
    public class UpdateCategoryDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// نام جدید دسته بندی
        /// </summary>
        public string NewCategoryName { get; set; } = string.Empty;
    }
}
