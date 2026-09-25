using Application.Dtos.Category;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        #region DI
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<ActionResult> GetAllCategoryAsync()
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddCategoryAsync(AddCategoryDto category)
        {
            var result = await _categoryService.AddCategoryAsync(category);

            return StatusCode((int)result.StatusCode, result); 
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdateCategoryAsync(UpdateCategoryDto category)
        {
            var result = await _categoryService.UpdateCategoryAsync(category);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
