using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        #region Get list for admin
        /// <summary>
        /// سبد خرید تمامی کاربران را برای ادمین به صورت صفحه بندی شده دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست سبد خرید تمامی کاربران</returns>
        Task<IEnumerable<AdminCartDto>> GetAllCartsForAdminAsync(QueryStringParameters parameters);
        
        /// <summary>
        /// آیتم های سبد خرید تمامی کاربران را برای ادمین دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست آیتم های سبد خرید تمام کاربران</returns>
        Task<IEnumerable<AdminCartItemDto>> GetAllCartItemsForAdminAsync
            (QueryStringParameters parameters);
        #endregion

        #region Get list
        /// <summary>
        /// برای مشتری دریافت میکند Dto آیتم های سبد خرید را به صورت
        /// </summary>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <returns>لیست آیتم های سبد</returns>
        Task<IEnumerable<CustomerCartItemDto>> GetCustomerCartItemsAsync(int cartId);
        
        /// <summary>
        /// آیتم های یک سبد خرید را دریافت میکند
        /// </summary>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <returns>لیست آیتم های سبد</returns>
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        
        /// <summary>
        /// آیتم های مرتبط با یک محصول را دریافت میکند
        /// </summary>
        /// <param name="productId">شناسه محصول</param>
        /// <returns>لیست آیتم های سبد مرتبط با محصول</returns>
        Task<IEnumerable<CartItem>> GetCartItemsByProductIdAsync(int productId);
        #endregion

        #region Get
        /// <summary>
        /// سبد خرید یک کاربر را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>یا سبد کاربر null</returns>
        Task<Cart?> GetCartAsync(string userId);
        
        /// <summary>
        /// یک آیتم سبد را به وسیله شناسه آیتم و شناسه سبد دریافت میکند
        /// </summary>
        /// <param name="cartItemId">شناسه آیتم سبد</param>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <returns>یا آیتم پیداشده null</returns>
        Task<CartItem?> GetCartItemAsync(int cartItemId, int cartId);
        
        /// <summary>
        /// اطلاعات نمایشی یک آیتم سبد را برای کاربر دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemId">شناسه آیتم سبد</param>
        /// <returns>یا آیتم پیداشده null</returns>
        Task<CustomerCartItemDto?> GetCustomerCartItemDtoAsync(string userId, int cartItemId);
        
        /// <summary>
        /// آیتم یک محصول مشخص را در یک سبد پیدا میکند
        /// </summary>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <param name="productId">شناسه محصول</param>
        /// <returns>یا آیتم پیداشده null</returns>
        Task<CartItem?> GetCartItemByProductIdAsync(int cartId, int productId);
        #endregion

        #region Add
        /// <summary>
        /// سبد خرید جدیدی را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="cart">سبد خرید جدید</param>
        Task AddCartAsync(Cart cart);

        /// <summary>
        /// آیتم جدیدی را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="cartItem">آیتم سبد جدید</param>
        Task AddCartItemAsync(CartItem cartItem);
        #endregion

        #region Update
        /// <summary>
        /// آیتم سبد را برای به روزرسانی علامتگذاری میکند
        /// </summary>
        /// <param name="cartItem">آیتم به روزشده</param>
        Task UpdateCartItemAsync(CartItem cartItem);
        #endregion

        #region Delete
        /// <summary>
        /// یک آیتم سبد را حذف میکند
        /// </summary>
        /// <param name="cartItem">آیتم مورد نظر برای حذف</param>
        Task DeleteCartItemAsync(CartItem cartItem);
        /// <summary>
        /// چند آیتم سبد را حذف میکند
        /// </summary>
        /// <param name="cartItems">آیتم های مورد نظر برای حذف</param>
        Task DeleteCartItemsAsync(IEnumerable<CartItem> cartItems);
        #endregion
    }
}
