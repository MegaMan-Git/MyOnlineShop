using Application.Entities;
using Application.Filters;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// مربوط به خواندن و مدیریت دسته بندی ها در پایگاه داده Repository
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        #region DI
        private readonly OnlineShopContext _context;

        /// <summary>
        /// دریافت میکند Dependency Injection را از طریق Repository وابستگی مورد نیاز
        /// </summary>
        public CategoryRepository(OnlineShopContext context)
        {
            _context = context;
        }
        #endregion

        #region GetCategory
        /// <summary>
        /// دسته بندی های حذف نشده را به صورت صفحه بندی شده دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست دسته بندی ها</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var categories = _context.Categories
                .Where(c => c.IsDeleted == false);
            #endregion

            #region Sorting 
            categories =
                parameters.SortDirection == "asc" ?
                    categories.OrderBy(c => c.Title).ThenBy(c => c.Id) :
                    categories.OrderByDescending(c => c.Title).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            categories = categories
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await categories.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// یک دسته بندی حذف نشده را بر اساس شناسه دریافت میکند
        /// </summary>
        /// <param name="id">شناسه دسته بندی</param>
        /// <returns>یا دسته بندی پیدا شده null</returns>
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);
        }
        #endregion

        #region AddCategory
        /// <summary>
        /// دسته بندی جدیدی را جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="category">دسته بندی جدید</param>
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }
        #endregion

        #region UpdateCategory
        /// <summary>
        /// دسته بندی را برای به روزرسانی علامت گذاری میکند
        /// </summary>
        /// <param name="category">دسته بندی به روزشده</param>
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
        }
        #endregion

        #region IsDuplicateCategoryName
        /// <summary>
        /// بررسی میکند که آیا دسته بندی ای با نام مشخص شده وجود دارد یا نه
        /// (برای افزودن دسته بندی)
        /// </summary>
        /// <param name="categoryName">نام دسته بندی</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        public async Task<bool> IsDuplicateCategoryNameAsync(string categoryName)
        {
            return await _context.Categories
                .AnyAsync(c => c.Title == categoryName);
        }

        /// <summary>
        /// بررسی میکند که آیا دسته بندی دیگری با این نام وجود دارد یا نه
        /// (برای بروزرسانی دسته بندی)
        /// </summary>
        /// <param name="categoryName">نام دسته بندی</param>
        /// <param name="categoryId">شناسه دسته بندی فعلی که باید نادیده گرفته شود</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        public async Task<bool> IsDuplicateCategoryNameAsync
            (string categoryName, int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.Title == categoryName && c.Id != categoryId);
        }
        #endregion
    }
}
