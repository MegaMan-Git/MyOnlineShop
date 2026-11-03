using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineShop.Api.Controllers
{
    /// <summary>
    /// کنترلر مربوط به مدیریت سبد خرید کاربران و مدیران
    /// </summary>
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        #region DI
        private readonly ICartService _cartService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// وابستگی های مورد نیاز کنترلر سبد خرید را دریافت میکند
        /// </summary>
        /// <param name="cartService">سرویس مدیریت سبد خرید</param>
        /// <param name="validationService">سرویس اعتبارسنجی اطلاعات ورودی</param>
        public CartController(ICartService cartService,
            IValidationService validationService)
        {
            _cartService = cartService;
            _validationService = validationService;
        }
        #endregion

        #region Get UserId
        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId!;
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
                result.Message = ".ورودی ارسالی نامعتبر است";

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

        #region Get Admin
        /// <summary>
        /// اکشن دریافت فهرست سبدهای خرید برای ادمین
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سبد های خرید یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<ActionResult> GetAllCartsAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var result = await _cartService.GetAllCartsForAdminAsync(parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت فهرست آیتم های سبدهای خرید برای ادمین
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست آیتم ها یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/items")]
        public async Task<ActionResult> GetAllCartItemsAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var result = await _cartService.GetAllCartItemsForAdminAsync(parameters);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Get
        /// <summary>
        /// اکشن دریافت آیتم های سبد خرید برای کاربر
        /// </summary>
        /// <returns>نتیجه شامل آیتم های سبد خرید یا پیام خطا</returns>
        [HttpGet]
        public async Task<ActionResult> GetCartItemsAsync()
        {
            var userId = GetUserId();

            var result = await _cartService.GetCustomerCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت یک آیتم سبد خرید مشخص شده
        /// </summary>
        /// <param name="id">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه شامل آیتم سبد خرید یا پیام خطا</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCartItemAsync(int id)
        {
            var userId = GetUserId();

            var result = await _cartService.GetCustomerCartItemAsync(userId, id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        /// <summary>
        /// اکشن افزودن یک آیتم به سبد خرید
        /// </summary>
        /// <param name="cartItemDto">اطلاعات محصول و تعداد مورد نیاز</param>
        /// <returns>نتیجه شامل آیتم اضافه شده یا پیام خطا</returns>
        [HttpPost]
        public async Task<ActionResult> AddCartItem(AddCartItemDto cartItemDto)
        {
            var userId = GetUserId();

            var result = await ValidateModelAsync<CustomerCartItemDto, AddCartItemDto>(cartItemDto);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _cartService.AddCartItemAsync(userId, cartItemDto);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        /// <summary>
        /// اکشن بروزرسانی یک آیتم در سبد خرید
        /// </summary>
        /// <param name="cartItemDto">شناسه آیتم و تعداد جدید</param>
        /// <returns>نتیجه شامل آیتم به روز شده یا پیام خطا</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateCartItem(UpdateCartItemDto cartItemDto)
        {
            var userId = GetUserId();

            var result = await ValidateModelAsync<CustomerCartItemDto, UpdateCartItemDto>(cartItemDto);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _cartService.UpdateCartItemAsync(userId, cartItemDto);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        /// <summary>
        /// اکشن حذف تمامی آیتم های سبد خرید
        /// </summary>
        /// <returns>نتیجه حذف آیتم ها</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteCartItemsAsync()
        {
            var userId = GetUserId();

            var result = await _cartService.ClearCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن حذف یک آیتم از سبد خرید با شناسه آیتم
        /// </summary>
        /// <param name="id">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه حذف آیتم</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItemAsync(int id)
        {
            var userId = GetUserId();

            var result = await _cartService.DeleteCartItemAsync(userId, id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
