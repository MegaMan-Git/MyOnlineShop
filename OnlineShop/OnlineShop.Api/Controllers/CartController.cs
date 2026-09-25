using Application.Dtos.Cart.Cartitem;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }
        #endregion

        #region Get UserId
        private async Task<string> GetUserId()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email!);
            
            return user!.Id;
        }
        #endregion

        #region Get Admin
        [HttpGet("admin/cart")]
        public async Task<ActionResult> GetAllCartsAsync()
        {
            var result = await _cartService.GetAllCartsForAdminAsync();

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("admin/cartItem")]
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
            var userId = await GetUserId();

            var result = await _cartService.GetCustomerCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCartItemAsync(int id)
        {
            var userId = await GetUserId();

            var result = await _cartService.GetCustomerCartItemAsync(userId, id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddCartItem(AddCartItemDto cartItemDto)
        {
            var userId = await GetUserId();

            var result = await _cartService.AddCartItemAsync(userId, cartItemDto);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdateCartItem(UpdateCartItemDto cartItemDto)
        {
            var userId = await GetUserId();

            var result = await _cartService.UpdateCartItemAsync(userId, cartItemDto);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<ActionResult> DeleteCartItemsAsync()
        {
            var userId = await GetUserId();
            
            var result = await _cartService.ClearCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItemAsync(int id)
        {
            var userId = await GetUserId(); 

            var result = await _cartService.DeleteCartItemAsync(userId, id);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
