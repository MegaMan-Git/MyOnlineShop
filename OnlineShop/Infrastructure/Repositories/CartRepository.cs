using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        #region DI
        private readonly OnlineShopContext _context;
        public CartRepository(OnlineShopContext context)
        {
            _context = context;

        }
        #endregion

        #region Get AllCart&CartItem
        public async Task<IEnumerable<AdminCartDto>> GetAllCartsForAdminAsync()
        {
            return await (
                from cart in _context.Carts
                join user in _context.Users
                    on cart.UserId equals user.Id
                select new AdminCartDto
                {
                    Id = cart.Id,
                    UserName = user.UserName
                }).ToListAsync();
        }

        public async Task<IEnumerable<AdminCartItemDto>> GetAllCartItemsForAdminAsync()
        {
            return await (
                from cart in _context.Carts
                join cartitem in _context.CartItems
                    on cart.Id equals cartitem.CartId
                join product in _context.Products
                    on cartitem.ProductId equals product.Id
                join user in _context.Users
                    on cart.UserId equals user.Id
                select new AdminCartItemDto
                {
                    Id = cartitem.Id,
                    CartId = cart.Id,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    UserName = user.UserName,
                    Quantity = cartitem.Quantity
                }).ToListAsync();
        }
        #endregion

        #region Get Cart&CartItems
        public async Task<Cart?> GetCartAsync(string userId)
        {
            return await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartItemId, int cartId)
        {
            return await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.CartId == cartId);
        }

        public async Task<CartItem?> GetCartItemByProductIdAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<IEnumerable<CustomerCartItemDto>> GetCartItemsAsync(int cartId)
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
                }).ToListAsync();
        }

        #endregion

        #region Add Cart&CartItem
        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Update CartItem
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Cart&CartItem
        public async Task DeleteCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
