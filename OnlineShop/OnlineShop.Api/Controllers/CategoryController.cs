using Application.Dtos.Category;
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
    /// کنترلر مربوط به مدیریت دسته بندی های محصولات
    /// </summary>
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region DI
        private readonly ICategoryService _categoryService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// وابستگی های مورد نیاز کنترلر دسته بندی را دریافت میکند
        /// </summary>
        /// <param name="categoryService">سرویس مدیریت دسته بندی</param>
        /// <param name="validationService">سرویس اعتبارسنجی اطلاعات ورودی</param>
        public CategoryController(ICategoryService categoryService, IValidationService validationService)
        {
            _categoryService = categoryService;
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
        /// اکشن دریافت فهرست دسته بندی ها با صفحه بندی و مرتب سازی
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست دسته بندی ها یا پیام خطا</returns>
        [Authorize(Roles = "Admin,Seller,Customer")]
        [HttpGet]
        public async Task<ActionResult> GetAllCategoryAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var result = await _categoryService.GetAllCategoriesAsync(parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت دسته بندی مشخص شده
        /// </summary>
        /// <param name="id">شناسه دسته بندی</param>
        /// <returns>نتیجه شامل دسته بندی یا پیام خطا</returns>
        [Authorize(Roles = "Admin,Seller,Customer")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryByIdAsync(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        /// <summary>
        /// اکشن افزودن یک دسته بندی جدید
        /// </summary>
        /// <param name="category">اطلاعات دسته بندی جدید</param>
        /// <returns>نتیجه شامل دسته بندی ایجاد شده یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCategoryAsync(AddCategoryDto category)
        {
            var result = await ValidateModelAsync<CategoryDto, AddCategoryDto>(category);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _categoryService.AddCategoryAsync(category);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        /// <summary>
        /// اکشن بروزرسانی نام یک دسته بندی
        /// </summary>
        /// <param name="category">شناسه و نام جدید دسته بندی</param>
        /// <returns>نتیجه شامل دسته بندی به روز شده یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateCategoryAsync(UpdateCategoryDto category)
        {
            var result = await ValidateModelAsync<CategoryDto, UpdateCategoryDto>(category);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _categoryService.UpdateCategoryAsync(category);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        /// <summary>
        /// اکشن حذف یک دسته بندی به صورت نرم
        /// </summary>
        /// <param name="id">شناسه دسته بندی</param>
        /// <returns>نتیجه حذف دسته بندی</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _categoryService.SoftDeleteCategoryAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
