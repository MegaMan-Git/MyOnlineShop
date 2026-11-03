using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Infrastructure.Persistence.Context;

namespace Infrastructure.UnitOfWork
{
    /// <summary>
    ///  ها و ذخیره تغییرات پایگاه داده را هماهنگ میکند Repository دسترسی به 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineShopContext _context;

        public IProductRepository productRepository { get; }
        public ICategoryRepository categoryRepository { get; }
        public ICartRepository cartRepository { get; }
        public IOrderRepository orderRepository { get; }

        public UnitOfWork(
            OnlineShopContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ICartRepository cartRepository,
            IOrderRepository orderRepository)
        {
            _context = context;

            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.cartRepository = cartRepository;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// را در پایگاه داده ذخیره میکند DbContext تمام تغییرات ثبت شده در 
        /// </summary>
        /// <returns>تعداد رکوردهای تغییرکرده</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
