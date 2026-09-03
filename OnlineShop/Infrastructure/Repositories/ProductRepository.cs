using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region DI
        private readonly OnlineShopContext _context;
        public ProductRepository(OnlineShopContext context)
        {
            _context = context;
        }
        #endregion

        #region GetProduct
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .ToListAsync();
        }
        #endregion

        #region AddProduct
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdateProduct
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DeleteProduct
        public async Task DeleteProductAsync(Product product)
        {
           _context.Products.Remove(product);
           await _context.SaveChangesAsync();
        }
        #endregion

        #region IsProductNameDuplicate
        public async Task<bool> IsProductNameDuplicateAsync(string productName)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == productName);
        }
        #endregion
    }
}
