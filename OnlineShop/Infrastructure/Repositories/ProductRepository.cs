using Application.Filters;
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
        public async Task<IEnumerable<Product>> GetAllProductsAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var products = _context.Products
                .AsNoTracking()
                .Where(c => c.IsDeleted == false);
            #endregion

            #region Sorting 
            products =
                parameters.SortDirection == "asc" ?
                    products.OrderBy(c => c.ProductName).ThenBy(c => c.Id) : 
                    products.OrderByDescending(c => c.ProductName).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            products = products
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
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
