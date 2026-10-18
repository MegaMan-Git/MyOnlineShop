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
        
        //get for admin
        Task<AdminOrderDto?> GetOrderForAdminAsync(int Orderid);

        //get list
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int OrderId);
        Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync(string userId);
        Task<IEnumerable<Payment>> GetPaymentsAsync(string userId);

        //get
        Task<Order?> GetOrderAsync(int orderId, string userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<CustomerOrderItemDto?> GetOrderItemAsync(int OrderItemId, int OrderId);
        Task<Payment?> GetPaymentAsync(int orderId);
        Task<Payment?> GetPaymentByAuthorityAsync(string Authority);

        //add
        Task AddOrderAsync(Order Order);

        //update
        Task UpdateOrderAsync(Order order);
    }
}
