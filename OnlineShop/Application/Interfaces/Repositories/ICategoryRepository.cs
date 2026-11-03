using Application.Entities;
using Application.Filters;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        #region Get
        /// <summary>
        /// دسته بندی های حذف نشده را به صورت صفحه بندی شده دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست دسته بندی ها</returns>
        Task<IEnumerable<Category>> GetAllCategoriesAsync(QueryStringParameters parameters);
        /// <summary>
        /// یک دسته بندی حذف نشده را بر اساس شناسه دریافت میکند
        /// </summary>
        /// <param name="id">شناسه دسته بندی</param>
        /// <returns>یا دسته بندی پیدا شده null</returns>
        Task<Category?> GetCategoryByIdAsync(int id);
        #endregion

        #region Add
        /// <summary>
        /// دسته بندی جدیدی را جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="category">دسته بندی جدید</param>
        Task AddCategoryAsync(Category category);
        #endregion

        #region Update
        //update
        /// <summary>
        /// دسته بندی را برای به روزرسانی علامت گذاری میکند
        /// </summary>
        /// <param name="category">دسته بندی به روزشده</param>
        Task UpdateCategoryAsync(Category category);
        #endregion

        #region Check
        /// <summary>
        /// بررسی میکند که آیا دسته بندی ای با نام مشخص شده وجود دارد یا نه
        /// (برای افزودن دسته بندی)
        /// </summary>
        /// <param name="categoryName">نام دسته بندی</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        Task<bool> IsDuplicateCategoryNameAsync(string categoryName);
        /// <summary>
        /// بررسی میکند که آیا دسته بندی دیگری با این نام وجود دارد یا نه
        /// (برای بروزرسانی دسته بندی)
        /// </summary>
        /// <param name="categoryName">نام دسته بندی</param>
        /// <param name="categoryId">شناسه دسته بندی فعلی که باید نادیده گرفته شود</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        Task<bool> IsDuplicateCategoryNameAsync(string categoryName, int categoryId);
        #endregion
    }
}
