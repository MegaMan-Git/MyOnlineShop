using Application.Dtos.Product;
using Application.Filters;
using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        /// <summary>
        /// فهرست محصولات را با مرتب سازی و صفحه بندی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست محصولات یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<ProductDto>>> GetAllProductsAsync
            (QueryStringParameters parameters);
        
        /// <summary>
        /// محصولی را با شناسه آن دریافت میکند
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه شامل محصول یا پیام خطا</returns>
        Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id);

        /// <summary>
        /// محصول جدیدی را به فروشگاه اضافه میکند
        /// </summary>
        /// <param name="productDto">اطلاعات محصول جدید</param>
        /// <returns>نتیجه شامل محصول ایجادشده یا پیام خطا</returns>
        Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto productDto);

        /// <summary>
        /// اطلاعات محصول را به روزرسانی میکند
        /// </summary>
        /// <param name="productDto">اطلاعات جدید محصول</param>
        /// <returns>نتیجه شامل محصول به روزشده یا پیام خطا</returns>
        Task<ServiceResult<ProductDto>> UpdateProductAsync(UpdateProductDto productDto);

        /// <summary>
        /// محصول را به صورت نرم حذف کرده و آیتم های مربوط به آن را از سبدهای خرید حذف میکند
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه حذف محصول</returns>
        Task<ServiceResult<string>> SoftDeleteProductAsync(int id);
    }
}
