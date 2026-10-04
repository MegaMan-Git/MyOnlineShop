using Application.Dtos.Cart.Cartitem;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineShop.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        #region DI
        private readonly ICartService _cartService;
        private readonly IValidationService _validationService;
        public CartController(ICartService cartService,
            IValidationService validationService)
        {
            _cartService = cartService;
            _validationService = validationService;
        }
        #endregion

        #region Get UserId
        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return userId!;
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

        #region Get Admin
        [HttpGet("admin")]
        public async Task<ActionResult> GetAllCartsAsync()
        {
            var result = await _cartService.GetAllCartsForAdminAsync();

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("admin/items")]
        public async Task<ActionResult> GetAllCartItemsAsync()
        {
            var result = await _cartService.GetAllCartItemsForAdminAsync();

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<ActionResult> GetCartItemsAsync()
        {
            var userId = GetUserId();

            var result = await _cartService.GetCustomerCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCartItemAsync(int id)
        {
            var userId =  GetUserId();

            var result = await _cartService.GetCustomerCartItemAsync(userId, id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddCartItem(AddCartItemDto cartItemDto)
        {
            var userId = GetUserId();

            var result = await ValidateModelAsync<CustomerCartItemDto, AddCartItemDto>(cartItemDto);

            if(result.StatusCode == ResultStatusCode.Success)
            {
                result = await _cartService.AddCartItemAsync(userId, cartItemDto);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdateCartItem(UpdateCartItemDto cartItemDto)
        {
            var userId = GetUserId();

            var result = await ValidateModelAsync<CustomerCartItemDto, UpdateCartItemDto>(cartItemDto);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _cartService.UpdateCartItemAsync(userId, cartItemDto);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<ActionResult> DeleteCartItemsAsync()
        {
            var userId = GetUserId();
            
            var result = await _cartService.ClearCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItemAsync(int id)
        {
            var userId = GetUserId(); 

            var result = await _cartService.DeleteCartItemAsync(userId, id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}