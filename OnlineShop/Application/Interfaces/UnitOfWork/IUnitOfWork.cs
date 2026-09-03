using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICartRepository cartRepository { get; }
        ICategoryRepository categoryRepository { get; }
        IOrderRepository orderRepository { get; }
        IProductRepository productRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
