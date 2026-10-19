using Application.Dtos.Cart.Cartitem;
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
        Task<ServiceResult<IEnumerable<AdminOrderDto>>> GetAllOrdersForAdminAsync();
        Task<ServiceResult<IEnumerable<AdminOrderItemDto>>> GetAllOrderItemsForAdminAsync();
        Task<ServiceResult<IEnumerable<CustomerOrderDto>>> GetOrdersAsync(string userId);
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> GetOrderItemsAsync(string userId,int orderId);
        Task<ServiceResult<CustomerOrderItemDto>> GetOrderItemAsync(string userId,int orderItemId, int orderId);
        Task<ServiceResult<PaymentDto>> GetPaymentAsync(string userId, int orderId);
        Task<ServiceResult<IEnumerable<PaymentDto>>> GetPaymentsAsync(string userId);

        //add order
        Task<ServiceResult<CustomerOrderItemDto>> CreateOrderFromCartItemAsync
            (string userId, int cartItemId);
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> CreateOrderFromCartItemsAsync
            (string userId);

        //payment
        Task<ServiceResult<string>> StartPaymentAsync(string userId, int orderId, string callbackUrl);
        Task<ServiceResult<string>> VerifyPaymentAsync(string authority, string gatewayStatus);
        Task<ServiceResult<PaymentDto>> UpdatePaymentStatusAsync(ChangePaymentStatusDto changePaymentStatus);

        //update order
        Task<ServiceResult<AdminOrderDto>> UpdateOrderStatusAsync(ChangeOrderStatusDto changeOrderStatus);
    }
}
