using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync();
        Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync();
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int OrderId);
        Task<Order?> GetOrderAsync(int orderId, string userId);
        Task<CustomerOrderItemDto?> GetOrderItemAsync(int OrderItemId, int OrderId);
        Task<Payment?> GetPaymentAsync(int orderId);
        
        Task AddOrderAsync(Order Order);
        Task AddOrderItemAsync(OrderItem OrderItem);
        Task AddPaymentAsync(int orderId,int totalPrice);
        
        Task UpdatePaymentStatusAsync(Payment payment);

    }
}
