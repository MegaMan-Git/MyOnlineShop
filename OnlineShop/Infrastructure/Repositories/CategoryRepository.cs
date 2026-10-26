using Application.Entities;
using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        #region DI
        private readonly OnlineShopContext _context;

        public CategoryRepository(OnlineShopContext context)
        {
            _context = context;
        }
        #endregion

        #region GetCategory
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync
            (QueryStringParameters parameters)
        {
            #region Lazy Query
            var categories = _context.Categories
                .AsNoTracking()
                .Where(c => c.IsDeleted == false);
            #endregion

            #region Sorting 
            categories =
                parameters.SortDirection == "asc" ?
                    categories.OrderBy(c => c.Title).ThenBy(c => c.Id) : 
                    categories.OrderByDescending(c => c.Title).ThenByDescending(c => c.Id);
            #endregion

            #region Pagination
            categories = categories
                .Skip(parameters.PageSize * (parameters.PageNumber - 1))
                .Take(parameters.PageSize);
            #endregion

            //execute and return
            return await categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);
        }
        #endregion

        #region AddCategory
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }
        #endregion

        #region UpdateCategory
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
        }
        #endregion

        #region IsDuplicateCategoryName
        public async Task<bool> IsDuplicateCategoryNameAsync(string categoryName)
        {
            return await _context.Categories
                .AnyAsync(c => c.Title == categoryName);
        }

        public async Task<bool> IsDuplicateCategoryNameAsync(string categoryName, int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.Title == categoryName && c.Id != categoryId);
        }
        #endregion
    }
}
