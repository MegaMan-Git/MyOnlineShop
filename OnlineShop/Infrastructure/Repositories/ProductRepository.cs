using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// مربوط به خواندن و مدیریت محصولات در پایگاه داده Repository
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        #region DI
        private readonly OnlineShopContext _context;

        /// <summary>
        /// دریافت میکند Dependency Injection را از طریق Repository وابستگی مورد نیاز
        /// </summary>
        public ProductRepository(OnlineShopContext context)
        {
            _context = context;
        }
        #endregion

        #region GetProduct 
        /// <summary>
        /// محصولات حذف نشده را به صورت صفحه بندی شده دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست محصولات</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var products = _context.Products
                .AsNoTracking()
                .Where(c => c.IsDeleted == false);
            #endregion

            #region Sorting 
            products =
                parameters.SortDirection == "asc" ?
                    products.OrderBy(c => c.ProductName).ThenBy(c => c.Id) :
                    products.OrderByDescending(c => c.ProductName).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            products = products
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await products.ToListAsync();
        }

        /// <summary>
        /// محصولات مرتبط با یک دسته بندی مشخص را دریافت میکند
        /// </summary>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <returns>لیست محصولاتی که آن دسته بندی را دارند</returns>
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        /// <summary>
        /// یک محصول حذف نشده را بر اساس شناسه دریافت میکند
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>یا محصول پیداشده null</returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }
        #endregion

        #region AddProduct
        /// <summary>
        /// محصول جدید را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="product">محصولی که قرار است اضافه شود</param>
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }
        #endregion

        #region UpdateProduct
        /// <summary>
        /// محصول را برای به روزرسانی علامت گذاری میکند
        /// </summary>
        /// <param name="product">محصول بروز شده</param>
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
        }
        #endregion

        #region IsProductNameDuplicate
        /// <summary>
        /// بررسی میکند که آیا محصولی با نام مشخص وجود دارد یا نه
        /// (برای افزودن محصول)
        /// </summary>
        /// <param name="productName">نام محصول</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        public async Task<bool> IsProductNameDuplicateAsync(string productName)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == productName);
        }
        
        /// <summary>
        /// بررسی میکند که آیا محصول دیگری با این نام وجود دارد یا نه
        /// (برای بروزرسانی محصول)
        /// </summary>
        /// <param name="productName">نام محصول</param>
        /// <param name="productId">شناسه محصول فعلی که باید نادیده گرفته شود</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        public async Task<bool> IsProductNameDuplicateAsync(string productName, int productId)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductName == productName && p.Id != productId);
        }
        #endregion
    }
}
