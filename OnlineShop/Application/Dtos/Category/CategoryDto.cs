namespace Application.Dtos.Category
{
    /// <summary>
    /// مدل نمایش دسته بندی
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// نام دسته بندی
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}
