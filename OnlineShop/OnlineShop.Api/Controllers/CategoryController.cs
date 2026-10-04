using Application.Dtos.Category;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
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
        private readonly IValidationService _validationService;
        public CategoryController(ICategoryService categoryService, IValidationService validationService)
        {
            _categoryService = categoryService;
            _validationService = validationService;
        }
        #endregion

        #region ValidationMethod
        private async Task<ServiceResult<TResponse>> ValidateModelAsync<TResponse, TModel>(TModel model)
        {
            var result = new ServiceResult<TResponse>();

            var validationResult = await _validationService.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                result.StatusCode = ResultStatusCode.BadRequest;
                result.Message = "ورودی ارسالی نامعتبر است";

                foreach (var error in validationResult.Errors)
                {
                    result.Errors.Add(error.ErrorMessage);
                }

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;

            return result;
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
        public async Task<ActionResult> GetCategoryByIdAsync(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddCategoryAsync(AddCategoryDto category)
        {
            var result = await ValidateModelAsync<CategoryDto, AddCategoryDto>(category);

            if(result.StatusCode == ResultStatusCode.Success)
            {
                result = await _categoryService.AddCategoryAsync(category);
            }

            return StatusCode((int)result.StatusCode, result); 
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdateCategoryAsync(UpdateCategoryDto category)
        {
            var result = await ValidateModelAsync<CategoryDto, UpdateCategoryDto>(category);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _categoryService.UpdateCategoryAsync(category);
            }

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
