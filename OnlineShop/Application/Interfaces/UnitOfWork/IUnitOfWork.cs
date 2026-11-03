using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICartRepository cartRepository { get; }
        ICategoryRepository categoryRepository { get; }
        IOrderRepository orderRepository { get; }
        IProductRepository productRepository { get; }

        /// <summary>
        /// را در پایگاه داده ذخیره میکند DbContext تمام تغییرات ثبت شده در 
        /// </summary>
        /// <returns>تعداد رکوردهای تغییر کرده</returns>
        Task<int> SaveChangesAsync();
    }
}
