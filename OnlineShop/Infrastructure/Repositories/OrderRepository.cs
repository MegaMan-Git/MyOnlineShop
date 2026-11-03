using Application.Dtos.Order;
using Application.Dtos.Order.OrderItem;
using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// مربوط به خواندن و مدیریت سفارش ها، آیتم های سفارش و پرداخت ها در پایگاه داده Repository
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        #region DI
        private readonly OnlineShopContext _context;

        /// <summary>
        /// دریافت میکند Dependency Injection را از طریق Repository وابستگی مورد نیاز
        /// </summary>
        /// <param name="context">Context مربوط به پایگاه داده</param>
        public OrderRepository(OnlineShopContext context)
        {
            _context = context;
        }
        #endregion

        #region Get AllOrders&OrderItems
        /// <summary>
        /// سفارش های موجود را برای ادمین با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست سفارش تمامی کاربران</returns>
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

        /// <summary>
        /// آیتم های سفارش ها را برای ادمین با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست آیتم های سفارش تمامی کاربران</returns>
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
        /// <summary>
        /// اطلاعات یک سفارش را برای ادمین دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>یا اطلاعات سفارش null</returns>
        public async Task<AdminOrderDto?> GetOrderForAdminAsync(int orderId)
        {
            return await (
                from order in _context.Orders
                join user in _context.Users
                    on order.UserId equals user.Id
                join payment in _context.Payments
                    on order.Id equals payment.OrderId
                where order.Id == orderId
                select new AdminOrderDto
                {
                    OrderId = order.Id,
                    UserName = user.UserName,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt,
                    Authority = payment.Authority,
                    ReferenceId = payment.ReferenceId
                }).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// تمام آیتم های یک سفارش را دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>لیست آیتم های سفارش</returns>
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

        /// <summary>
        /// آیتم های یک سفارش را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست صفحه بندی شده آیتم های سفارش</returns>
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

        /// <summary>
        /// سفارش های یک کاربر را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست صفحه بندی شده سفارش ها</returns>
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

        /// <summary>
        /// آیتم های یک سفارش را همراه با محصولاتشان دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>آیتم های سفارش همراه با محصول مرتبط</returns>
        public async Task<IEnumerable<OrderItem>> GetOrderItemsIncludeProductsAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId).ToListAsync();
        }

        /// <summary>
        /// پرداخت های یک کاربر را با صفحه بندی و مرتب سازی دریافت میکند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="parameters">پارامترهای صفحه بندی و مرتب سازی</param>
        /// <returns>لیست پرداخت های کاربر</returns>
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

        /// <summary>
        /// سفارش یک کاربر را بر اساس شناسه سفارش و شناسه کاربر دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>یا سفارش null</returns>
        public async Task<Order?> GetOrderAsync(int orderId, string userId)
        {
            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        /// <summary>
        /// یک سفارش را بر اساس شناسه دریافت میکند و به صورت غیر مستقیم از آن استفاده میشود
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>یا سفارش null</returns>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        /// <summary>
        /// یک آیتم مشخص را از یک سفارش دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="orderItemId">شناسه آیتم سفارش</param>
        /// <returns>یا آیتم سفارش null</returns>
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

        /// <summary>
        /// پرداخت مربوط به یک سفارش را دریافت میکند
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns>یا پرداخت null</returns>
        public async Task<Payment?> GetPaymentAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        /// <summary>
        /// دریافت میکند Authority پرداخت را بر اساس مقدار
        /// </summary>
        /// <param name="authority">شناسه پرداخت دریافتی از درگاه</param>
        /// <returns>یا پرداخت null</returns>
        public async Task<Payment?> GetPaymentByAuthorityAsync(string authority)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Authority == authority);
        }
        #endregion

        #region Add Order
        /// <summary>
        /// سفارش جدید را به جدول پایگاه داده اضافه میکند
        /// </summary>
        /// <param name="order">سفارش جدید</param>
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        #endregion
    }
}
