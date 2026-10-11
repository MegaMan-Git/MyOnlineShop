using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        //get list
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);

        //get
        Task<Product?> GetProductByIdAsync(int id);
        
        //add
        Task AddProductAsync(Product product);  
        
        //update
        Task UpdateProductAsync(Product product);
        
        //check
        Task<bool> IsProductNameDuplicateAsync(string productName);
        Task<bool> IsProductNameDuplicateAsync(string productName,int productId);
    }
}
