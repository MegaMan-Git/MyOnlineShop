using Application.Dtos.Product;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace OnlineShop.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        #region DI
        private readonly IProductService _productService;
        private readonly IValidationService _validationService;
        public ProductController(IProductService productService, IValidationService validationService)
        {
            _productService = productService;
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
        public async Task<ActionResult> GetAllProductsAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductByIdAsync(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return StatusCode((int)result.StatusCode,result);
        }

        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddProductAsync(AddProductDto product)
        {
            var result = await ValidateModelAsync<ProductDto, AddProductDto>(product);

            if(result.StatusCode == ResultStatusCode.Success)
            {
                result = await _productService.AddProductAsync(product);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdateProductAsync(UpdateProductDto product)
        {
            var result = await ValidateModelAsync<ProductDto, UpdateProductDto>(product);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _productService.UpdateProductAsync(product);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
