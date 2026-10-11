using Application.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        //get
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);

        //add
        Task AddCategoryAsync(Category category);
        
        //update
        Task UpdateCategoryAsync(Category category); 
        
        //check
        Task<bool> IsDuplicateCategoryNameAsync(string categoryName);
        Task<bool> IsDuplicateCategoryNameAsync(string categoryName, int categoryId);

    }
}
