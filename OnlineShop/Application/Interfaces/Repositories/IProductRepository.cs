using Application.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        #region Get list
        /// <summary>
        /// محصولات حذف نشده را به صورت صفحه بندی شده دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست محصولات</returns>
        
        Task<IEnumerable<Product>> GetAllProductsAsync(QueryStringParameters parameters);
        /// <summary>
        /// محصولات مرتبط با یک دسته بندی مشخص را دریافت میکند
        /// </summary>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <returns>لیست محصولاتی که آن دسته بندی را دارند</returns>
        Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);
        #endregion

        #region Get
        /// <summary>
        /// یک محصول حذف نشده را بر اساس شناسه دریافت میکند
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>یا محصول پیداشده null</returns>
        Task<Product?> GetProductByIdAsync(int id);
        #endregion

        #region Add
        /// <summary>
        /// محصول جدید را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="product">محصولی که قرار است اضافه شود</param>
        Task AddProductAsync(Product product);
        #endregion

        #region Update
        /// <summary>
        /// محصول را برای به روزرسانی علامت گذاری میکند
        /// </summary>
        /// <param name="product">محصول بروز شده</param>
        Task UpdateProductAsync(Product product);
        #endregion

        #region Check
        /// <summary>
        /// بررسی میکند که آیا محصولی با نام مشخص وجود دارد یا نه
        /// (برای افزودن محصول)
        /// </summary>
        /// <param name="productName">نام محصول</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        Task<bool> IsProductNameDuplicateAsync(string productName);
        
        /// <summary>
        /// بررسی میکند که آیا محصول دیگری با این نام وجود دارد یا نه
        /// (برای بروزرسانی محصول)
        /// </summary>
        /// <param name="productName">نام محصول</param>
        /// <param name="productId">شناسه محصول فعلی که باید نادیده گرفته شود</param>
        /// <returns>در صورت وجود نام تکراری true</returns>
        Task<bool> IsProductNameDuplicateAsync(string productName, int productId);
        #endregion
    }
}
