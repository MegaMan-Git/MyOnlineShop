using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Dtos.Payment;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync();
        Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync();
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int OrderId);
        Task<CustomerOrderItemDto?> GetOrderItemAsync(int OrderItemId, int OrderId);
        Task<Payment?> GetPaymentAsync(int orderId);

        
        Task<int> AddOrderAsync(Order Order);
        Task AddOrderItemAsync(OrderItem OrderItem);
        Task AddPaymentAsync(int orderId,int totalPrice);
        
        Task UpdatePaymentStatusAsync(Payment payment);

    }
}
