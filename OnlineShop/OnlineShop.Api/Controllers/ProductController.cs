using Application.Dtos.Product;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers
{
    /// <summary>
    /// کنترلر مربوط به مدیریت محصولات
    /// </summary>
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region DI
        private readonly IProductService _productService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// وابستگی های مورد نیاز کنترلر محصول را دریافت میکند
        /// </summary>
        /// <param name="productService">سرویس مدیریت محصولات</param>
        /// <param name="validationService">سرویس اعتبارسنجی اطلاعات ورودی</param>
        public ProductController(IProductService productService, IValidationService validationService)
        {
            _productService = productService;
            _validationService = validationService;
        }
        #endregion

        #region ValidationMethod
        private async Task<ServiceResult<TResponse>> ValidateModelAsync<TResponse, TModel>(TModel model)
        {
            var result = new ServiceResult<TResponse>();

            var validationResult = await _validationService.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = "ورودی ارسالی نامعتبر است";

                foreach (var error in validationResult.Errors)
                {
                    result.Errors.Add(error.ErrorMessage);
                }

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;

            return result;
        }
        #endregion

        #region Get
        /// <summary>
        /// اکشن دریافت فهرست محصولات با صفحه بندی و مرتب سازی
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست محصولات یا پیام خطا</returns>
        [Authorize(Roles = "Admin,Seller,Customer")]
        [HttpGet]
        public async Task<ActionResult> GetAllProductsAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var result = await _productService.GetAllProductsAsync(parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت یک محصول مشخص شده
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه شامل محصول یا پیام خطا</returns>
        [Authorize(Roles = "Admin,Seller,Customer")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductByIdAsync(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region Post
        /// <summary>
        /// اکشن افزودن یک محصول جدید
        /// </summary>
        /// <param name="product">اطلاعات محصول جدید</param>
        /// <returns>نتیجه شامل محصول ایجاد شده یا پیام خطا</returns>
        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        public async Task<ActionResult> AddProductAsync(AddProductDto product)
        {
            var result = await ValidateModelAsync<ProductDto, AddProductDto>(product);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _productService.AddProductAsync(product);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        /// <summary>
        /// اکشن بروزرسانی اطلاعات یک محصول
        /// </summary>
        /// <param name="product">اطلاعات جدید محصول</param>
        /// <returns>نتیجه شامل محصول به روز شده یا پیام خطا</returns>
        [Authorize(Roles = "Admin,Seller")]
        [HttpPut]
        public async Task<ActionResult> UpdateProductAsync(UpdateProductDto product)
        {
            var result = await ValidateModelAsync<ProductDto, UpdateProductDto>(product);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _productService.UpdateProductAsync(product);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        /// <summary>
        /// اکشن حذف یک محصول به صورت نرم
        /// </summary>
        /// <param name="id">شناسه محصول</param>
        /// <returns>نتیجه حذف محصول</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var result = await _productService.SoftDeleteProductAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
