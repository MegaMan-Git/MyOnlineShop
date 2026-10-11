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
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(c => c.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }
        #endregion

        #region AddProduct
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }
        #endregion

        #region UpdateProduct
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
        }
        #endregion

        #region IsProductNameDuplicate
        public async Task<bool> IsProductNameDuplicateAsync(string productName)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == productName);
        }
        public async Task<bool> IsProductNameDuplicateAsync(string productName, int productId)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == productName && p.Id != productId);
        }
        #endregion
    }
}
