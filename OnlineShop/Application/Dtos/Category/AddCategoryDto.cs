namespace Application.Dtos.Category
{
    /// <summary>
    /// مدل افزودن دسته بندی
    /// </summary>
    public class AddCategoryDto
    {
        /// <summary>
        /// نام دسته بندی
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}
