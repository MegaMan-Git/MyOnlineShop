using Application.Dtos.Auth;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers
{
    /// <summary>
    /// کنترلر مربوط به اندپوینت های احراز هویت
    /// </summary>
    
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region DI
        private readonly IAuthService _authService;
        private readonly IValidationService _validationService;

        /// <summary>
        /// وابستگی های مورد نیاز کنترلر احراز هویت را دریافت میکند
        /// </summary>
        /// <param name="authService">سرویس مدیریت کاربران</param>
        /// <param name="validationService">سرویس اعتبارسنجی اطلاعات ورودی</param>
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
                result.Message = ".ورودی ارسالی نامعتبر است";

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
        /// <summary>
        /// اکشن لاگین کاربر
        /// </summary>
        /// <param name="login">اطلاعات ورود شامل: ایمیل و رمزعبور</param>
        /// <returns>درصورت احراز هویت موفق jwt توکن</returns>
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
        /// <summary>
        /// اکشن ثبت نام کاربر
        /// </summary>
        /// <param name="register">اطلاعات ثبت نام: نام کاربری، ایمیل، رمزعبور و شماره تلفن</param>
        /// <returns>درصورت احراز هویت موفق jwt توکن</returns>
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
        /// <summary>
        /// اکشن فراموشی رمز عبور
        /// </summary>
        /// <param name="forgotPassword">ایمیل ثبت نام شده</param>
        /// <returns>توکن تعویض رمزعبور اگر ایمیل موجود باشد</returns>
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPassword)
        {
            var result = await ValidateModelAsync<ResetTokenResponseDto, ForgotPasswordDto>(forgotPassword);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.ForgotPasswordAsync(forgotPassword);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// اکشن تعویض رمز عبور
        /// </summary>
        /// <param name="resetPassword">اطلاعات مورد نیاز: ایمیل، توکن و رمزعبور جدید</param>
        /// <returns>پیغام موفقیت یا خطا</returns>
        [HttpPut("resetpassword")]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var result = await ValidateModelAsync<string, ResetPasswordDto>(resetPassword);
            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.ResetPasswordAsync(resetPassword);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region GetUserRoles
        /// <summary>
        /// اکشن دریافت نقش تمامی کاربران
        /// </summary>
        /// <returns>فهرست تمام کاربران شامل ایمیل و نقش</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("user/roles")]
        public async Task<ActionResult> GetUserRolesAsync()
        {
            var result = await _authService.GetUserRolesAsync();

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region ChangeUserRole
        /// <summary>
        /// اکشن بروزرسانی نقش کاربر
        /// </summary>
        /// <param name="roleChange">اطلاعات مورد نیاز شامل: ایمیل و نقش جدید</param>
        /// <returns>پیغام موفقیت یا خطا</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("user/roles")]
        public async Task<ActionResult> UpdateUserRole(RoleChangeDto roleChange)
        {
            var result = await ValidateModelAsync<string, RoleChangeDto>(roleChange);

            if (result.StatusCode == ResultStatusCode.Success)
            {
                result = await _authService.ChangeUserRoleAsync(roleChange);
            }

            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
