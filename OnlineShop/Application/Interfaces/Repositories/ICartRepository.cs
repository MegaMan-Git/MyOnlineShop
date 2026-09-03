using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICartRepository
    {

        Task<IEnumerable<AdminCartDto>> GetAllCartsForAdminAsync();
        Task<IEnumerable<AdminCartItemDto>> GetAllCartItemsForAdminAsync();
        Task<IEnumerable<CustomerCartItemDto>> GetCartItemsAsync(int cartId);
        Task<Cart?> GetCartAsync(string userId);
        Task<CartItem?> GetCartItemAsync(int cartItemId, int cartId);
        Task<CartItem?>GetCartItemByProductIdAsync(int cartId,int productId);
       
        Task AddCartAsync(Cart cart);
        Task AddCartItemAsync(CartItem cartItem);
        
        Task UpdateCartItemAsync(CartItem cartItem);
        
        Task DeleteCartAsync(Cart cart);
        Task DeleteCartItemAsync(CartItem cartItem);

    }
}
