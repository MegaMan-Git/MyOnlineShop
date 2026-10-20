using Application.Dtos.Cart;
using Application.Dtos.Cart.Cartitem;
using Application.Filters;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{  
    public interface ICartService
    {
        //get list admin
        Task<ServiceResult<IEnumerable<AdminCartDto>>> GetAllCartsForAdminAsync
            (QueryStringParameters parameters);
        Task<ServiceResult<IEnumerable<AdminCartItemDto>>> GetAllCartItemsForAdminAsync
            (QueryStringParameters parameters);
        
        //get list
        Task<ServiceResult<IEnumerable<CustomerCartItemDto>>> GetCustomerCartItemsAsync
            (string userId);
        
        //get
        Task<ServiceResult<CustomerCartItemDto>> GetCustomerCartItemAsync
            (string userId, int cartItemId);

        //add
        Task<ServiceResult<CustomerCartItemDto>> AddCartItemAsync
            (string userId,AddCartItemDto cartItemDto);

        //update
        Task<ServiceResult<CustomerCartItemDto>> UpdateCartItemAsync
            (string userId ,UpdateCartItemDto cartItemDto);
    
        //delete
        Task<ServiceResult<string>> DeleteCartItemAsync(string userId,int cartItemId);
        Task<ServiceResult<string>> ClearCartItemsAsync(string userId);
            
    }
}
