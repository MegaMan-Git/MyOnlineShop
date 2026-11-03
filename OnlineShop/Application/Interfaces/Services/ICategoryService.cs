using Application.Dtos.Category;
using Application.Filters;
using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// فهرست دسته بندی ها را با مرتب سازی و صفحه بندی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست دسته بندی ها یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync
            (QueryStringParameters parameters);
        
        /// <summary>
        /// دسته بندی ای را با شناسه آن دریافت میکند
        /// </summary>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <returns>نتیجه شامل دسته بندی یا پیام خطا</returns>
        Task<ServiceResult<CategoryDto>> GetCategoryByIdAsync(int categoryId);

        /// <summary>
        /// دسته بندی جدیدی ایجاد میکند
        /// </summary>
        /// <param name="categoryDto">اطلاعات دسته بندی جدید</param>
        /// <returns>نتیجه شامل دسته بندی ایجادشده یا پیام خطا</returns>
        Task<ServiceResult<CategoryDto>> AddCategoryAsync(AddCategoryDto categoryDto);

        /// <summary>
        /// اطلاعات دسته بندی را به روزرسانی میکند
        /// </summary>
        /// <param name="categoryDto">اطلاعات جدید دسته بندی</param>
        /// <returns>نتیجه شامل دسته بندی به روزشده یا پیام خطا</returns>
        Task<ServiceResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto categoryDto);

        /// <summary>
        /// دسته بندی را به صورت نرم حذف میکند
        /// </summary>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <returns>نتیجه حذف دسته بندی</returns>
        Task<ServiceResult<string>> SoftDeleteCategoryAsync(int categoryId);
    }
}
