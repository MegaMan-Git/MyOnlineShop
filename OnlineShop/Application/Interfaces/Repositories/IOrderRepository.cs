using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        #region Get list for admin 
        /// <summary>
        /// سفارش های موجود را برای ادمین با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست سفارش تمامی کاربران</returns>
        Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync
            (QueryStringParameters parameters);

        /// <summary>
        /// آیتم های سفارش ها را برای ادمین با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست آیتم های سفارش تمامی کاربران</returns>
        Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync
            (QueryStringParameters parameters);
        #endregion

        #region Get for admin
        /// <summary>
        /// اطلاعات یک سفارش را برای ادمین دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>یا اطلاعات سفارش null</returns>
        Task<AdminOrderDto?> GetOrderForAdminAsync(int orderId);
        #endregion

        #region Get list
        /// <summary>
        /// تمام آیتم های یک سفارش را دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>لیست آیتم های سفارش</returns>
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int orderId);

        /// <summary>
        /// آیتم های یک سفارش را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست صفحه بندی شده آیتم های سفارش</returns>
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync
            (int orderId, QueryStringParameters parameters);

        /// <summary>
        /// سفارش های یک کاربر را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست صفحه بندی شده سفارش ها</returns>
        Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync
            (string userId, QueryStringParameters parameters);

        /// <summary>
        /// آیتم های یک سفارش را همراه با محصولاتشان دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>آیتم های سفارش همراه با محصول مرتبط</returns>
        Task<IEnumerable<OrderItem>> GetOrderItemsIncludeProductsAsync(int orderId);

        /// <summary>
        /// پرداخت های یک کاربر را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست پرداخت های کاربر</returns>
        Task<IEnumerable<Payment>> GetPaymentsAsync
            (string userId, QueryStringParameters parameters);
        #endregion

        #region Get
        /// <summary>
        /// سفارش یک کاربر را بر اساس شناسه سفارش و شناسه کاربر دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>یا سفارش null</returns>
        Task<Order?> GetOrderAsync(int orderId, string userId);
        
        /// <summary>
        /// یک سفارش را بر اساس شناسه دریافت میکند و به صورت غیر مستقیم از آن استفاده میشود
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>یا سفارش null</returns>
        Task<Order?> GetOrderByIdAsync(int orderId);
        
        /// <summary>
        /// یک آیتم مشخص را از یک سفارش دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="orderItemId">شناسه آیتم سفارش</param>
        /// <returns>یا آیتم سفارش null</returns>
        Task<CustomerOrderItemDto?> GetOrderItemAsync(int orderItemId, int orderId);
        
        /// <summary>
        /// پرداخت مربوط به یک سفارش را دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>یا پرداخت null</returns>
        Task<Payment?> GetPaymentAsync(int orderId);
        
        /// <summary>
        /// دریافت میکند Authority پرداخت را بر اساس مقدار
        /// </summary>
        /// <param name="authority">شناسه پرداخت دریافتی از درگاه</param>
        /// <returns>یا پرداخت null</returns>
        Task<Payment?> GetPaymentByAuthorityAsync(string authority);
        #endregion

        #region Add
        /// <summary>
        /// سفارش جدید را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="order">سفارش جدید</param>
        Task AddOrderAsync(Order order);
        #endregion
    }
}
