using Application.Dtos.Category;
using Application.Entities;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly OnlineShopContext _context;

        public CategoryRepository(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsDuplicateCategoryNameAsync(string newCategoryName)
        {
            return await _context.Categories
                .AnyAsync(c => c.Title == newCategoryName);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
