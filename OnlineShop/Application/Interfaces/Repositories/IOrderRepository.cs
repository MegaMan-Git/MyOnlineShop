using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        //get list for admin 
        Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync();
        Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync();
        
        //get list
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int OrderId);
        Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync(string userId);
        Task<IEnumerable<Payment>> GetPaymentsAsync(string userId);

        //get
        Task<Order?> GetOrderAsync(int orderId, string userId);
        Task<CustomerOrderItemDto?> GetOrderItemAsync(int OrderItemId, int OrderId);
        Task<Payment?> GetPaymentAsync(int orderId);

        //add
        Task AddOrderAsync(Order Order);
        
        //update
        Task UpdatePaymentStatusAsync(Payment payment);

    }
}
