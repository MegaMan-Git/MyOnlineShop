using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Dtos.Payment;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

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

        #region Get AllOrder&OrderItem
        public async Task<IEnumerable<AdminOrderDto>> GetAllOrdersForAdminAsync()
        {
            return await (
                from order in _context.Orders
                join user in _context.Users
                    on order.UserId equals user.Id
                select new AdminOrderDto
                {
                    Id = order.Id,
                    UserName = user.UserName
                }).ToListAsync();
        }

        public async Task<IEnumerable<AdminOrderItemDto>> GetAllOrderItemsForAdminAsync()
        {
            return await (
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
                    Price = product.Price,
                    TotalPrice = product.Price * orderitem.Quantity,
                    Quantity = orderitem.Quantity
                }).ToListAsync();
        }
        #endregion

        #region Get OrderItems&Payment
        public async Task<CustomerOrderItemDto?> GetOrderItemAsync(int orderItemId, int orderId)
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
                }).FirstOrDefaultAsync();
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
                }).ToListAsync();
        }

        public async Task<Payment?> GetPaymentAsync(int orderId)
        {
            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        #endregion

        #region Add Order&OrderItem
        public async Task<int> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Payment
        public async Task AddPaymentAsync(int orderId, int totalPrice)
        {
            await _context.Payments.AddAsync(new Payment
            {
                Amount = totalPrice,
                OrderId= orderId,
                CreatedAt = DateTime.UtcNow,
            });
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Update PaymentStatus
        public async Task UpdatePaymentStatusAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
