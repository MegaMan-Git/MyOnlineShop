using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{  
    public interface ICartService
    {
        Task<ServiceResult<IEnumerable<AdminCartDto>>> GetAllCartsForAdminAsync();
        Task<ServiceResult<IEnumerable<AdminCartItemDto>>> GetAllCartItemsForAdminAsync();
        Task<ServiceResult<IEnumerable<CustomerCartItemDto>>> GetCustomerCartItemsAsync(string userId);
        Task<ServiceResult<CustomerCartItemDto>> GetCustomerCartItemAsync(string userId, int cartItemId);

        Task<ServiceResult<CustomerCartItemDto>> AddCartItemAsync(string userId,AddCartDto cartDto);

        Task<ServiceResult<CustomerCartItemDto>> UpdateCartItemAsync(string userId ,UpdateCartItemDto cartItemDto);
    
        Task<ServiceResult<string>> DeleteCartItemAsync(string userId,int cartItemId);
        Task<ServiceResult<string>> ClearCartItemsAsync(string userId);
            
    }
}