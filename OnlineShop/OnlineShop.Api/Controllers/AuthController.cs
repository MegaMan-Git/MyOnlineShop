using Application.Dtos.Auth;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
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
        private readonly IValidationService _validationService;
        public AuthController(IAuthService authService, IValidationService validationService)
        {
            _authService = authService;
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

        #region SignIn
        [HttpPost("signin")]
        public async Task<ActionResult> SignInAsync(LoginDto login)
        {
            var result = await ValidateModelAsync<JwtResponseDto, LoginDto>(login);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.SignInAsync(login);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region SignUp
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync(RegisterDto register)
        {
            var result = await ValidateModelAsync<JwtResponseDto, RegisterDto>(register);
            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.SignUpAsync(register);

                if (result.StatusCode != ResultStatusCode.Success)
                {
                    return StatusCode((int)result.StatusCode, result);
                }

                return StatusCode(201, result);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region Forgot&Reset Password
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPassword)
        {
            var result = await ValidateModelAsync<ResetTokenResponseDto, ForgotPasswordDto>(forgotPassword);
            
            if(result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.ForgotPasswordAsync(forgotPassword);
            }
            
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("resetpassword")]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var result = await ValidateModelAsync<JwtResponseDto, ResetPasswordDto>(resetPassword);
            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.ResetPasswordAsync(resetPassword);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
