using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        //get list for admin 
        Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync
            (QueryStringParameters parameters);
        Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync
            (QueryStringParameters parameters);
        
        //get for admin
        Task<AdminOrderDto?> GetOrderForAdminAsync(int orderId);

        //get list
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int orderId);
        Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync
            (int orderId, QueryStringParameters parameters); 
        Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync
            (string userId, QueryStringParameters parameters);
        Task<IEnumerable<OrderItem>> GetOrderItemsIncludeProductsAsync(int orderId);
        Task<IEnumerable<Payment>> GetPaymentsAsync
            (string userId, QueryStringParameters parameters);

        //get
        Task<Order?> GetOrderAsync(int orderId, string userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<CustomerOrderItemDto?> GetOrderItemAsync(int orderItemId, int orderId);
        Task<Payment?> GetPaymentAsync(int orderId);
        Task<Payment?> GetPaymentByAuthorityAsync(string authority);

        //add
        Task AddOrderAsync(Order order);

        //update
        Task UpdateOrderAsync(Order order);
    }
}
