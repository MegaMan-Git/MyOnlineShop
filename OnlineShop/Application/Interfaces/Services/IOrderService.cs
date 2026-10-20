using Application.Filters;
using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Dtos.Payment;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        //get list admin
        Task<ServiceResult<IEnumerable<AdminOrderDto>>> GetAllOrdersForAdminAsync
            (QueryStringParameters parameters);
        Task<ServiceResult<IEnumerable<AdminOrderItemDto>>> GetAllOrderItemsForAdminAsync
            (QueryStringParameters parameters);
        
        //get list
        Task<ServiceResult<IEnumerable<CustomerOrderDto>>> GetOrdersAsync
            (string userId, QueryStringParameters parameters);
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> GetOrderItemsAsync
            (string userId, int orderId, QueryStringParameters parameters);
        Task<ServiceResult<IEnumerable<PaymentDto>>> GetPaymentsAsync
            (string userId, QueryStringParameters parameters);

        //get 
        Task<ServiceResult<CustomerOrderItemDto>> GetOrderItemAsync
            (string userId,int orderItemId, int orderId);
        Task<ServiceResult<PaymentDto>> GetPaymentAsync(string userId, int orderId);
        

        //add
        Task<ServiceResult<CustomerOrderItemDto>> CreateOrderFromCartItemAsync
            (string userId, int cartItemId);
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> CreateOrderFromCartItemsAsync
            (string userId);

        //payment
        Task<ServiceResult<string>> StartPaymentAsync
            (string userId, int orderId, string callbackUrl);
        Task<ServiceResult<string>> VerifyPaymentAsync
            (string authority, string gatewayStatus);

        //update
        Task<ServiceResult<PaymentDto>> UpdatePaymentStatusAsync
            (ChangePaymentStatusDto changePaymentStatus);
        Task<ServiceResult<AdminOrderDto>> UpdateOrderStatusAsync
            (ChangeOrderStatusDto changeOrderStatus);
    }
}
