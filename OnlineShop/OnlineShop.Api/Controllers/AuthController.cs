using Application.Dtos.Auth;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace OnlineShop.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region DI
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region SignIn
        [HttpPost("signin")]
        public async Task<ActionResult> SignInAsync(LoginDto login)
        {
            var result = await _authService.SignInAsync(login);        

            return StatusCode((int)result.StatusCode,result);
        }
        #endregion

        #region SignUp
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync(RegisterDto register)
        {
            var result = await _authService.SignUpAsync(register);

            if (result.StatusCode != ResultStatusCode.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return StatusCode(201, result);
        }
        #endregion

        #region Forgot&Reset Password
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPassword)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPassword);

            return StatusCode((int)result.StatusCode,result);
        }

        [HttpPut("resetpassword")]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var result = await _authService.ResetPasswordAsync(resetPassword);

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
