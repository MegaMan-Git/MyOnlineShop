using Application.Dtos.Product;
using Application.Interfaces.Services;
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
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Get       
        [HttpGet]
        public async Task<ActionResult> GetAllProductsAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProductByIdAsync(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return StatusCode((int)result.StatusCode,result);
        }

        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddProductAysnc(AddProductDto product)
        {
            var result = await _productService.AddProductAsync(product);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdateProductAsync(UpdateProductDto product)
        {
            var result = await _productService.UpdateProductAsync(product);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
