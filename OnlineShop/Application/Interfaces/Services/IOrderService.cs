using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Dtos.Payment;
using Application.Filters;
using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        #region Get list for admin
        /// <summary>
        /// فهرست سفارش ها را برای ادمین با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سفارش ها یا پیام خطا</returns>       
        Task<ServiceResult<IEnumerable<AdminOrderDto>>> GetAllOrdersForAdminAsync
            (QueryStringParameters parameters);
        
        /// <summary>
        /// فهرست آیتم های سفارش ها را برای ادمین با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست آیتم ها یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<AdminOrderItemDto>>> GetAllOrderItemsForAdminAsync
            (QueryStringParameters parameters);
        #endregion

        #region Get list
        /// <summary>
        /// فهرست سفارش های متعلق به مشتری را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سفارش ها یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<CustomerOrderDto>>> GetOrdersAsync
            (string userId, QueryStringParameters parameters);
        
        /// <summary>
        /// آیتم های یک سفارش متعلق به مشتری را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل آیتم های سفارش یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> GetOrderItemsAsync
            (string userId, int orderId, QueryStringParameters parameters);
        
        /// <summary>
        /// فهرست پرداخت های مشتری را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست پرداخت ها یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<PaymentDto>>> GetPaymentsAsync
            (string userId, QueryStringParameters parameters);
        #endregion

        #region Get
        /// <summary>
        /// یک آیتم سفارش متعلق به مشتری را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="orderItemId">شناسه آیتم سفارش</param>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>نتیجه شامل آیتم سفارش یا پیام خطا</returns>
        Task<ServiceResult<CustomerOrderItemDto>> GetOrderItemAsync
            (string userId, int orderItemId, int orderId);
        
        /// <summary>
        /// اطلاعات پرداخت یک سفارش متعلق به مشتری را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>نتیجه شامل اطلاعات پرداخت یا پیام خطا</returns>
        Task<ServiceResult<PaymentDto>> GetPaymentAsync(string userId, int orderId);
        #endregion

        #region Add
        /// <summary>
        /// از یک آیتم سبد خرید، سفارش ایجاد میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="cartItemId">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه شامل آیتم سفارش ایجادشده یا پیام خطا</returns>
        Task<ServiceResult<CustomerOrderItemDto>> CreateOrderFromCartItemAsync
            (string userId, int cartItemId);
        /// <summary>
        /// از آیتم های سبد خرید، سفارش ایجاد میکند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <returns>نتیجه شامل آیتم های سفارش ایجادشده یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> CreateOrderFromCartItemsAsync
            (string userId);
        #endregion

        #region Payment
        /// <summary>
        /// برای سفارش، درخواست پرداخت ایجاد کرده و نشانی ورود به درگاه را برمیگرداند
        /// </summary>
        /// <param name="userId">شناسه مشتری</param>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="callbackUrl">نشانی بازگشت از درگاه پرداخت</param>
        /// <returns>نشانی ورود به درگاه یا پیام خطا</returns>
        Task<ServiceResult<string>> StartPaymentAsync
            (string userId, int orderId, string callbackUrl);
       
        /// <summary>
        /// نتیجه پرداخت را از درگاه دریافت و پرداخت را تایید میکند
        /// </summary>
        /// <param name="authority">شناسه پرداخت دریافت شده از درگاه</param>
        /// <param name="gatewayStatus">وضعیت بازگشتی درگاه</param>
        /// <returns>پرداخت موفق یا پیام خطا ReferenceId</returns>
        Task<ServiceResult<string>> VerifyPaymentAsync
            (string authority, string gatewayStatus);
        #endregion

        #region Update
        /// <summary>
        /// وضعیت پرداخت سفارش را تغییر میدهد و در صورت نیاز وضعیت سفارش و موجودی را هماهنگ میکند
        /// </summary>
        /// <param name="changePaymentStatus">شناسه سفارش و وضعیت پرداخت جدید</param>
        /// <returns>اطلاعات پرداخت به روزشده یا پیام خطا</returns>
        Task<ServiceResult<PaymentDto>> UpdatePaymentStatusAsync
            (ChangePaymentStatusDto changePaymentStatus);
        
        /// <summary>
        /// وضعیت سفارش را تغییر داده و موجودی و وضعیت پرداخت را هماهنگ میکند
        /// </summary>
        /// <param name="changeOrderStatus">شناسه سفارش و وضعیت جدید</param>
        /// <returns>اطلاعات سفارش به روزشده یا پیام خطا</returns>
        Task<ServiceResult<AdminOrderDto>> UpdateOrderStatusAsync
            (ChangeOrderStatusDto changeOrderStatus);
        #endregion
    }
}
