using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        
        Task AddProductAsync(Product product);  
    
        Task UpdateProductAsync(Product product);
        
        Task DeleteProductAsync(Product product);

        Task<bool> IsProductNameDuplicateAsync(string productName);
    }
}
