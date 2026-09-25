using Application.Dtos.Payment;
using Application.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineShop.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        #region DI
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
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
        [HttpGet("admin/order")]
        public async Task<ActionResult> GetAllOrderAsync()
        {
            var result = await _orderService.GetAllOrdersForAdminAsync();
        
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("admin/orderitem")]
        public async Task<ActionResult> GetAllOrderItemAsync()
        {
            var result = await _orderService.GetAllOrderItemsForAdminAsync();

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<ActionResult> GetOrdersAsync()
        {
            var userId = await GetUserId();

            var result = await _orderService.GetOrdersAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{orderid}")]
        public async Task<ActionResult> GetOrderItemAsync(int orderId)
        {
            var userId = await GetUserId();

            var result = await _orderService.GetOrderItemsAsync(userId, orderId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{orderid}/{orderitemid}")]
        public async Task<ActionResult> GetOrderItemAsync(int orderId,int orderItemId)
        {
            var userId = await GetUserId();

            var result = await _orderService.GetOrderItemAsync(userId,orderItemId,orderId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("payment")]
        public async Task<ActionResult> GetPaymentsAsync()
        {
            var userId = await GetUserId();

            var result = await _orderService.GetPaymentsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("payment/{orderid}")]
        public async Task<ActionResult> GetPaymentAsync(int orderId)
        {
            var userId = await GetUserId();

            var result = await _orderService.GetPaymentAsync(userId, orderId);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<ActionResult> AddOrderAsync()
        {
            var userId = await GetUserId();
            
            var result = await _orderService.CreateOrderFromCartItemsAsync(userId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{cartitemid}")]
        public async Task<ActionResult> AddOrderAsync(int cartItemId)
        {
            var userId = await GetUserId();

            var result = await _orderService.CreateOrderFromCartItemAsync(userId,cartItemId);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Put
        [HttpPut("payment")]
        public async Task<ActionResult> UpdatePaymentAsync(AdminChangeStatusDto adminChangeStatus)
        {
            var userId = await GetUserId();

            var result = await _orderService.UpdatePaymentStatusAsync(adminChangeStatus);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
