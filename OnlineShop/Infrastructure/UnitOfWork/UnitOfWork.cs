using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Infrastructure.Persistence.Context;

namespace Infrastructure.UnitOfWork
{
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

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
