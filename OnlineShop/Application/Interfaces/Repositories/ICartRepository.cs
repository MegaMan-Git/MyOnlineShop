using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        //admin
        Task<IEnumerable<AdminCartDto>> GetAllCartsForAdminAsync(QueryStringParameters parameters);
        Task<IEnumerable<AdminCartItemDto>> GetAllCartItemsForAdminAsync(QueryStringParameters parameters);
        
        //list
        Task<IEnumerable<CustomerCartItemDto>> GetCustomerCartItemsAsync(int cartId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        Task<IEnumerable<CartItem>> GetCartItemsByProductIdAsync(int productId);

        //one
        Task<Cart?> GetCartAsync(string userId);
        Task<CartItem?> GetCartItemAsync(int cartItemId, int cartId);
        Task<CustomerCartItemDto?> GetCustomerCartItemDtoAsync(string userId,int cartItemId);
        Task<CartItem?>GetCartItemByProductIdAsync(int cartId,int productId);
       
        //add
        Task AddCartAsync(Cart cart);
        Task AddCartItemAsync(CartItem cartItem);
        
        //update
        Task UpdateCartItemAsync(CartItem cartItem);
        
        //delete
        Task DeleteCartItemAsync(CartItem cartItem);
        Task DeleteCartItemsAsync(IEnumerable<CartItem> cartItems);
    }
}
