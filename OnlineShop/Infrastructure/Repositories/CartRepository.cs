using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// مربوط به خواندن و مدیریت سبدهای خرید و آیتم های آن ها در پایگاه داده Repository
    /// </summary>
    public class CartRepository : ICartRepository
    {
        #region DI
        private readonly OnlineShopContext _context;

        /// <summary>
        /// دریافت میکند Dependency Injection را از طریق Repository وابستگی مورد نیاز
        /// </summary>
        public CartRepository(OnlineShopContext context)
        {
            _context = context;
        }
        #endregion

        #region Get AllCart&CartItem
        /// <summary>
        /// سبد خرید تمامی کاربران را برای ادمین به صورت صفحه بندی شده دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست سبد خرید تمامی کاربران</returns>
        public async Task<IEnumerable<AdminCartDto>> GetAllCartsForAdminAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var carts =
                from cart in _context.Carts
                join user in _context.Users
                    on cart.UserId equals user.Id
                select new AdminCartDto
                {
                    Id = cart.Id,
                    UserName = user.UserName
                };
            #endregion

            #region Sorting 
            carts =
                parameters.SortDirection == "asc" ?
                    carts.OrderBy(c => c.UserName).ThenBy(c => c.Id) :
                    carts.OrderByDescending(c => c.UserName).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            carts = carts
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await carts.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// آیتم های سبد خرید تمامی کاربران را برای ادمین دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست آیتم های سبد خرید تمام کاربران</returns>
        public async Task<IEnumerable<AdminCartItemDto>> GetAllCartItemsForAdminAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var cartItems =
                from cart in _context.Carts
                join cartitem in _context.CartItems
                    on cart.Id equals cartitem.CartId
                join product in _context.Products
                    on cartitem.ProductId equals product.Id
                join user in _context.Users
                    on cart.UserId equals user.Id
                where product.IsDeleted == false
                select new AdminCartItemDto
                {
                    Id = cartitem.Id,
                    CartId = cart.Id,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    UserName = user.UserName,
                    Quantity = cartitem.Quantity
                };
            #endregion

            #region Sorting 
            cartItems =
                parameters.SortDirection == "asc" ?
                    cartItems.OrderBy(ci => ci.UserName).ThenBy(ci => ci.Id) :
                    cartItems.OrderByDescending(ci => ci.UserName).ThenByDescending(ci => ci.Id);
            #endregion

            #region Pagination
            cartItems = cartItems
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //exectue and return
            return await cartItems.AsNoTracking().ToListAsync();
        }
        #endregion

        #region Get Cart&CartItems
        /// <summary>
        /// آیتم های یک سبد خرید را دریافت میکند
        /// </summary>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <returns>لیست آیتم های سبد</returns>
        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// آیتم های مرتبط با یک محصول را دریافت میکند
        /// </summary>
        /// <param name="productId">شناسه محصول</param>
        /// <returns>لیست آیتم های سبد مرتبط با محصول</returns>
        public async Task<IEnumerable<CartItem>> GetCartItemsByProductIdAsync(int productId)
        {
            return await _context.CartItems
                .Where(ci => ci.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// سبد خرید یک کاربر را دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>یا سبد کاربر null</returns>
        public async Task<Cart?> GetCartAsync(string userId)
        {
            return await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        /// <summary>
        /// یک آیتم سبد را به وسیله شناسه آیتم و شناسه سبد دریافت میکند
        /// </summary>
        /// <param name="cartItemId">شناسه آیتم سبد</param>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <returns>یا آیتم پیداشده null</returns>
        public async Task<CartItem?> GetCartItemAsync(int cartItemId, int cartId)
        {
            return await _context.CartItems
                .AsNoTracking()
                .Include(p => p.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.CartId == cartId);
        }

        /// <summary>
        /// اطلاعات نمایشی یک آیتم سبد را برای کاربر دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="cartItemId">شناسه آیتم سبد</param>
        /// <returns>یا اطلاعات آیتم null</returns>
        public async Task<CustomerCartItemDto?> GetCustomerCartItemDtoAsync
            (string userId, int cartItemId)
        {


            return await (
                from cart in _context.Carts
                join cartitem in _context.CartItems
                    on cart.Id equals cartitem.CartId
                join product in _context.Products
                    on cartitem.ProductId equals product.Id
                where cartitem.Id == cartItemId && cart.UserId == userId
                select new CustomerCartItemDto
                {
                    Id = cartitem.Id,
                    CartId = cartitem.CartId,
                    ProductId = cartitem.ProductId,
                    ProductName = product.ProductName,
                    Quantity = cartitem.Quantity,
                    Price = product.Price,
                    TotalPrice = product.Price * cartitem.Quantity
                }
                ).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// آیتم یک محصول مشخص را در یک سبد پیدا میکند
        /// </summary>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <param name="productId">شناسه محصول</param>
        /// <returns>یا آیتم پیداشده null</returns>
        public async Task<CartItem?> GetCartItemByProductIdAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        /// <summary>
        /// برای مشتری دریافت میکند Dto آیتم های سبد خرید را به صورت
        /// </summary>
        /// <param name="cartId">شناسه سبد خرید</param>
        /// <returns>لیست آیتم های سبد</returns>
        public async Task<IEnumerable<CustomerCartItemDto>> GetCustomerCartItemsAsync(int cartId)
        {
            return await (
                from cartitem in _context.CartItems
                join product in _context.Products
                on cartitem.ProductId equals product.Id
                where cartitem.CartId == cartId
                select new CustomerCartItemDto
                {
                    Id = cartitem.Id,
                    CartId = cartitem.CartId,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Quantity = cartitem.Quantity,
                    Price = product.Price,
                    TotalPrice = product.Price * cartitem.Quantity
                }).AsNoTracking().ToListAsync();
        }

        #endregion

        #region Add Cart&CartItem
        /// <summary>
        /// سبد خرید جدیدی را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="cart">سبد خرید جدید</param>
        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        /// <summary>
        /// آیتم جدیدی را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="cartItem">آیتم سبد جدید</param>
        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }
        #endregion

        #region Update CartItem
        /// <summary>
        /// آیتم سبد را برای به روزرسانی علامتگذاری میکند
        /// </summary>
        /// <param name="cartItem">آیتم به روزشده</param>
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
        }
        #endregion

        #region Delete CartItem
        /// <summary>
        /// یک آیتم سبد را حذف میکند
        /// </summary>
        /// <param name="cartItem">آیتم مورد نظر برای حذف</param>
        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }

        /// <summary>
        /// چند آیتم سبد را حذف میکند
        /// </summary>
        /// <param name="cartItems">آیتم های مورد نظر برای حذف</param>
        public async Task DeleteCartItemsAsync(IEnumerable<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
        }
        #endregion
    }
}
