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
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> GetOrderItemsAsync(string userId,int OrderId);
        Task<ServiceResult<CustomerOrderItemDto>> GetOrderItemAsync(string userId,int OrderItemId, int OrderId);
        Task<ServiceResult<PaymentDto>> GetPaymentAsync(string userId, int orderId);
        Task<ServiceResult<IEnumerable<PaymentDto>>> GetPaymentsAsync(string userId);

        Task<ServiceResult<CustomerOrderItemDto>> CreateOrderFromCartItemAsync
            (string userId, int cartItemId);
        Task<ServiceResult<IEnumerable<CustomerOrderItemDto>>> CreateOrdersFromCartAsync
            (string userId);

        Task<ServiceResult<PaymentDto>> UpdatePaymentStatusAsync(AdminChangeStatusDto adminChangeStatus);
    }
}
