using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int id);
        
        Task<IEnumerable<Product>> GetProductsAsync();
        
        Task<bool> IsProductNameDuplicateAsync(string productName);
        Task AddProductAsync(Product product);  
        Task UpdateProductAsync(Product product);
        
        Task DeleteProductAsync(Product product);
    }
}
