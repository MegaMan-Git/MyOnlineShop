using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.Security;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        #region DI
        private readonly OnlineShopContext _context;
        public OrderRepository(OnlineShopContext context)
        {
            _context = context;

        }
        #endregion
        
        #region Get AllOrders&OrderItems
        public async Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var orders = 
                from order in _context.Orders
                join payment in _context.Payments
                    on order.Id equals payment.OrderId
                join user in _context.Users
                    on order.UserId equals user.Id              
                select new AdminOrderDto
                {
                    OrderId = order.Id,
                    UserName = user.UserName,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt,
                    Authority = payment.Authority,
                    ReferenceId = payment.ReferenceId
                };
            #endregion

            #region Sorting 
            orders =
                parameters.SortDirection == "asc" ?
                    orders.OrderBy(c => c.UserName).ThenBy(c => c.OrderId) :
                    orders.OrderByDescending(c => c.UserName).ThenByDescending(c => c.OrderId);
            #endregion

            #region Pagination
            orders = orders
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await orders.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var orderItems =
                from order in _context.Orders
                join orderitem in _context.OrderItems
                    on order.Id equals orderitem.OrderId
                join product in _context.Products
                    on orderitem.ProductId equals product.Id
                join user in _context.Users
                    on order.UserId equals user.Id
                select new AdminOrderItemDto
                {
                    Id = orderitem.Id,
                    OrderId = order.Id,
                    UserName = user.UserName,
                    ProductName = product.ProductName,
                    Price = orderitem.UnitPrice,
                    TotalPrice = orderitem.UnitPrice * orderitem.Quantity,
                    Quantity = orderitem.Quantity
                };
            #endregion

            #region Sorting 
            orderItems =
                parameters.SortDirection == "asc" ?
                    orderItems.OrderBy(c => c.UserName).ThenBy(c => c.Id) :
                    orderItems.OrderByDescending(c => c.UserName).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            orderItems = orderItems
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await orderItems.AsNoTracking().ToListAsync();
        }
        #endregion

        #region Get Order&OrderItems&Payment
        public async Task<AdminOrderDto?> GetOrderForAdminAsync(int Orderid)
        {
            return await (
                from order in _context.Orders
                join user in _context.Users
                    on order.UserId equals user.Id
                join payment in _context.Payments
                    on order.Id equals payment.OrderId
                    where order.Id == Orderid
                select new AdminOrderDto
                    {
                        OrderId = order.Id,
                        UserName= user.UserName,
                        Status = order.Status,
                        CreatedAt = order.CreatedAt,
                        Authority = payment.Authority,
                        ReferenceId = payment.ReferenceId
                    }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync(int orderId)
        {
            return await (
                from orderitem in _context.OrderItems
                join product in _context.Products
                    on orderitem.ProductId equals product.Id
                where orderitem.OrderId == orderId
                select new CustomerOrderItemDto
                {
                    Id = orderitem.Id,
                    OrderId = orderitem.OrderId,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = orderitem.UnitPrice,
                    TotalPrice = orderitem.UnitPrice * orderitem.Quantity,
                    Quantity = orderitem.Quantity,
                }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CustomerOrderItemDto>> GetOrderItemsAsync
            (int orderId, QueryStringParameters parameters)
        {
            #region Lazy Query
            var orderItems =
                from orderitem in _context.OrderItems
                join product in _context.Products
                    on orderitem.ProductId equals product.Id
                where orderitem.OrderId == orderId
                select new CustomerOrderItemDto
                {
                    Id = orderitem.Id,
                    OrderId = orderitem.OrderId,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = orderitem.UnitPrice,
                    TotalPrice = orderitem.UnitPrice * orderitem.Quantity,
                    Quantity = orderitem.Quantity,
                };
            #endregion

            #region Sorting 
            orderItems =
                parameters.SortDirection == "asc" ?
                    orderItems.OrderBy(c => c.Id) : orderItems.OrderByDescending(c => c.Id);
            #endregion

            #region Pagination
            orderItems = orderItems
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await orderItems.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetCustomerOrdersAsync
            (string userId, QueryStringParameters parameters)
        {
            #region Lazy Query
            var orders =
                from order in _context.Orders
                join payment in _context.Payments
                    on order.Id equals payment.OrderId
                where order.UserId == userId
                select new CustomerOrderDto
                {
                    Id = order.Id,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt,
                    Authority = payment.Authority,
                    ReferenceId = payment.ReferenceId
                };
            #endregion

            #region Sorting
            orders =
                parameters.SortDirection == "asc" ?
                    orders.OrderBy(c => c.CreatedAt).ThenBy(c => c.Id) :
                    orders.OrderByDescending(c => c.CreatedAt).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            orders = orders
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await orders.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsIncludeProductsAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync
            (string userId, QueryStringParameters parameters)
        {
            #region Lazy Query
            var payments =
                from order in _context.Orders
                join payment in _context.Payments
                    on order.Id equals payment.OrderId
                where order.UserId == userId
                select new Payment
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    Status = payment.Status,
                    CreatedAt = payment.CreatedAt,
                    Authority = payment.Authority,
                    ReferenceId = payment.ReferenceId
                };
            #endregion

            #region Sorting 
            payments =
                parameters.SortDirection == "asc" ?
                    payments.OrderBy(p => p.Id) :
                    payments.OrderByDescending(c => c.Id);
            #endregion

            #region Pagination
            payments = payments
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await payments.AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetOrderAsync(int orderId, string userId)
        {
            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<CustomerOrderItemDto?> GetOrderItemAsync(int orderId, int orderItemId)
        {
            return await (
                from orderitem in _context.OrderItems
                join product in _context.Products
                    on orderitem.ProductId equals product.Id
                where orderitem.OrderId == orderId && orderitem.Id == orderItemId
                select new CustomerOrderItemDto
                {
                    Id = orderitem.Id,
                    OrderId = orderitem.OrderId,
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = orderitem.UnitPrice,
                    TotalPrice = orderitem.UnitPrice * orderitem.Quantity,
                    Quantity = orderitem.Quantity
                }).AsNoTracking().FirstOrDefaultAsync();
        } 

        public async Task<Payment?> GetPaymentAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<Payment?> GetPaymentByAuthorityAsync(string Authority)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Authority == Authority);
        }
        #endregion

        #region Add Order
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        #endregion

        #region Update Order
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
        } 
        #endregion
    }
}
