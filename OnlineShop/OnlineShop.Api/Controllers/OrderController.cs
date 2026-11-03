using Application.Dtos.Order;
using Application.Dtos.Payment;
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
    /// کنترلر مربوط به مدیریت سفارش ها و پرداخت ها
    /// </summary>
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        #region DI
        private readonly IOrderService _orderService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// وابستگی های مورد نیاز کنترلر سفارش را دریافت میکند
        /// </summary>
        /// <param name="orderService">سرویس مدیریت سفارش و پرداخت</param>
        /// <param name="validationService">سرویس اعتبارسنجی اطلاعات ورودی</param>
        public OrderController(IOrderService orderService,
            IValidationService validationService)
        {
            _orderService = orderService;
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
        /// اکشن دریافت فهرست سفارش ها را با صفحه بندی و مرتب سازی برای ادمین
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سفارش ها یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/order")]
        public async Task<ActionResult> GetAllOrderAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var result = await _orderService.GetAllOrdersForAdminAsync(parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت فهرست آیتم های سفارش ها را با صفحه بندی و مرتب سازی برای ادمین
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست آیتم ها یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/orderitem")]
        public async Task<ActionResult> GetAllOrderItemAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var result = await _orderService.GetAllOrderItemsForAdminAsync(parameters);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Get
        /// <summary>
        /// اکشن دریافت فهرست سفارش های کاربر با صفحه بندی و مرتب سازی
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سفارش ها یا پیام خطا</returns>
        [HttpGet]
        public async Task<ActionResult> GetOrdersAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var userId = GetUserId();

            var result = await _orderService.GetOrdersAsync(userId, parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت آیتم های یک سفارش با صفحه بندی و مرتب سازی
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل آیتم های سفارش یا پیام خطا</returns>
        [HttpGet("{orderid}")]
        public async Task<ActionResult> GetOrderItemsAsync
            (int orderId, [FromQuery] QueryStringParameters parameters)
        {
            var userId = GetUserId();

            var result = await _orderService.GetOrderItemsAsync(userId, orderId, parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت یک آیتم مشخص از سفارش
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="orderItemId">شناسه آیتم سفارش</param>
        /// <returns>نتیجه شامل آیتم سفارش یا پیام خطا</returns>
        [HttpGet("{orderid}/{orderitemid}")]
        public async Task<ActionResult> GetOrderItemAsync(int orderId, int orderItemId)
        {
            var userId = GetUserId();

            var result = await _orderService.GetOrderItemAsync(userId, orderItemId, orderId);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت فهرست پرداخت های کاربر با صفحه بندی و مرتب سازی
        /// </summary>
        /// <param name="parameters">پارامتر های صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست پرداخت ها یا پیام خطا</returns>
        [HttpGet("payment")]
        public async Task<ActionResult> GetPaymentsAsync
            ([FromQuery] QueryStringParameters parameters)
        {
            var userId = GetUserId();

            var result = await _orderService.GetPaymentsAsync(userId, parameters);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن دریافت اطلاعات پرداخت یک سفارش مشخص شده
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>نتیجه شامل اطلاعات پرداخت یا پیام خطا</returns>
        [HttpGet("{orderid}/payment")]
        public async Task<ActionResult> GetPaymentAsync(int orderId)
        {
            var userId = GetUserId();

            var result = await _orderService.GetPaymentAsync(userId, orderId);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        /// <summary>
        /// اکشن ایجاد یک سفارش از تمام آیتم های سبد خرید
        /// </summary>
        /// <returns>نتیجه شامل آیتم های سفارش ایجاد شده یا پیام خطا</returns>
        [HttpPost]
        public async Task<ActionResult> CreateOrderFromCartAsync()
        {
            var userId = GetUserId();

            var result = await _orderService.CreateOrderFromCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن ایجاد یک سفارش از یک آیتم سبد خرید مشخص شده
        /// </summary>
        /// <param name="cartItemId">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه شامل آیتم سفارش ایجاد شده یا پیام خطا</returns>
        [HttpPost("{cartitemid}")]
        public async Task<ActionResult> CreateOrderFromCartItemAsync(int cartItemId)
        {
            var userId = GetUserId();

            var result = await _orderService.CreateOrderFromCartItemAsync(userId, cartItemId);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        /// <summary>
        /// اکشن تغییر وضعیت یک سفارش مشخص شده
        /// </summary>
        /// <param name="changeOrderStatus">شناسه سفارش و وضعیت جدید</param>
        /// <returns>نتیجه شامل سفارش به روز شده یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateOrderStatusAsync(ChangeOrderStatusDto changeOrderStatus)
        {
            var result = await ValidateModelAsync<AdminOrderDto, ChangeOrderStatusDto>(changeOrderStatus);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _orderService.UpdateOrderStatusAsync(changeOrderStatus);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن تغییر وضعیت پرداخت یک سفارش مشخص شده
        /// </summary>
        /// <param name="changePaymentStatus">شناسه سفارش و وضعیت پرداخت جدید</param>
        /// <returns>نتیجه شامل پرداخت به روز شده یا پیام خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("payment")]
        public async Task<ActionResult> UpdatePaymentAsync(ChangePaymentStatusDto changePaymentStatus)
        {
            var result = await ValidateModelAsync<PaymentDto, ChangePaymentStatusDto>(changePaymentStatus);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _orderService.UpdatePaymentStatusAsync(changePaymentStatus);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Payment Process

        /// <summary>
        /// اکشن ایجاد درخواست پرداخت برای سفارش مشخص شده
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>نشانی ورود به درگاه یا پیام خطا</returns>
        [HttpPost("{orderId}/payment")]
        public async Task<ActionResult> StartPayment(int orderId)
        {
            var userId = GetUserId();

            var callbackUrl = $"{Request.Scheme}://{Request.Host}/api/order/payment/callback";

            var result = await _orderService.StartPaymentAsync(userId, orderId, callbackUrl);

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن بررسی نتیجه پرداخت از درگاه
        /// </summary>
        /// <param name="authority">شناسه پرداخت دریافت شده از درگاه</param>
        /// <param name="status">وضعیت بازگشتی درگاه</param>
        /// <returns>نتیجه تایید پرداخت یا پیام خطا</returns>
        [AllowAnonymous]
        [HttpGet("payment/callback")]
        public async Task<ActionResult> PaymentCallback
            ([FromQuery] string authority,
            [FromQuery] string status)
        {
            var result = await _orderService.VerifyPaymentAsync(authority, status);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
