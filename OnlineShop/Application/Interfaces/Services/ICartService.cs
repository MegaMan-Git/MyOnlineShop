using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface ICartService
    {
        #region Get list for admin
        /// <summary>
        /// فهرست سبدهای خرید را برای مدیر دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست سبدهای خرید یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<AdminCartDto>>> GetAllCartsForAdminAsync
            (QueryStringParameters parameters);
        
        /// <summary>
        /// فهرست آیتم های سبدهای خرید را برای مدیر دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>نتیجه شامل فهرست آیتم ها یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<AdminCartItemDto>>> GetAllCartItemsForAdminAsync
            (QueryStringParameters parameters);
        #endregion

        #region Get list
        /// <summary>
        /// آیتم های سبد خرید مشتری را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>نتیجه شامل آیتم های سبد خرید یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<CustomerCartItemDto>>> GetCustomerCartItemsAsync
            (string userId);
        #endregion

        #region Get
        /// <summary>
        /// یک آیتم مشخص از سبد خرید مشتری را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemId">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه شامل آیتم سبد خرید یا پیام خطا</returns>
        Task<ServiceResult<CustomerCartItemDto>> GetCustomerCartItemAsync
            (string userId, int cartItemId);
        #endregion

        #region Add
        /// <summary>
        /// محصولی را به سبد خرید مشتری اضافه میکند یا مقدار آیتم موجود را افزایش میدهد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemDto">محصول و تعداد موردنظر</param>
        /// <returns>نتیجه شامل آیتم ایجادشده یا به روزشده</returns>
        Task<ServiceResult<CustomerCartItemDto>> AddCartItemAsync
            (string userId, AddCartItemDto cartItemDto);
        #endregion

        #region update
        /// <summary>
        /// تعداد یک آیتم موجود در سبد خرید را تغییر میدهد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemDto">شناسه آیتم و تعداد جدید</param>
        /// <returns>نتیجه شامل آیتم به روزشده یا پیام خطا</returns>
        Task<ServiceResult<CustomerCartItemDto>> UpdateCartItemAsync
            (string userId, UpdateCartItemDto cartItemDto);
        #endregion

        #region Delete
        /// <summary>
        /// یک آیتم را از سبد خرید مشتری حذف میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemId">شناسه آیتم سبد خرید</param>
        /// <returns>نتیجه حذف آیتم</returns>
        Task<ServiceResult<string>> DeleteCartItemAsync(string userId, int cartItemId);
        
        /// <summary>
        /// تمام آیتم های سبد خرید مشتری را حذف میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>نتیجه حذف آیتم ها</returns>
        Task<ServiceResult<string>> ClearCartItemsAsync(string userId);
        #endregion
    }
}
