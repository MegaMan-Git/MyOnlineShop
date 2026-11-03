using Application.Dtos.Auth;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    /// <summary>
    /// منطق احراز هویت، ثبت نام، بازیابی رمز عبور و مدیریت نقش کاربران را اجرا میکند
    /// </summary>
    public class AuthService : IAuthService
    {
        #region DI
        private readonly IAuthRepository _authRepository;
        private readonly ITokenEncoder _tokenEncoder;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// وابستگی های مورد نیاز سرویس احراز هویت را دریافت میکند
        /// </summary>
        /// <param name="authRepository">برای انجام کوئری های مربوط به کاربران و نقش ها</param>
        /// <param name="tokenEncoder">برای کدگذاری و کدگشایی توکن بازیابی رمز عبور</param>
        /// <param name="jwtTokenGenerator">JWT برای ایجاد توکن</param>
        /// <param name="logger">برای ثبت رویدادها و خطاهای احراز هویت</param>
        public AuthService(IAuthRepository authRepository,
            ITokenEncoder tokenEncoder,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _tokenEncoder = tokenEncoder;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }
        #endregion

        #region SignIn
        /// <summary>
        /// ایجاد میکند JWT پس از ورود موفق
        /// </summary>
        /// <param name="loginDto">اطلاعات ورود کاربر</param>
        /// <returns>یا پیام خطا JWT نتیجه ورود شامل توکن</returns>
        public async Task<ServiceResult<JwtResponseDto>> SignInAsync(LoginDto loginDto)
        {
            var serviceResult = new ServiceResult<JwtResponseDto>();

            //find user
            var result = await _authRepository.SignInAsync(loginDto);

            if (result.IsSucceeded is false)
            {
                _logger.LogWarning("Sign-in failed for Email {Email}", loginDto.Email);

                serviceResult.Message = ".ایمیل یا رمز عبور صحیح نمیباشد";
                serviceResult.Errors = result.Errors;
                serviceResult.StatusCode = ResultStatusCode.Unauthorized;

                return serviceResult;
            }

            //generate jwt token
            string token = _jwtTokenGenerator.GenerateToken(loginDto.Email, result.UserId, result.Role);

            serviceResult.StatusCode = ResultStatusCode.Success;
            serviceResult.Data = new JwtResponseDto
            {
                Token = token
            };

            _logger.LogInformation("Sign-in succeeded for Email {Email}", loginDto.Email);

            return serviceResult;
        }
        #endregion

        #region SignUp
        /// <summary>
        /// ایجاد میکند JWT کاربر جدید را ثبت نام کرده و در صورت موفقیت توکن
        /// </summary>
        /// <param name="registerDto">اطلاعات ثبت نام کاربر</param>
        /// <returns>یا پیام خطا JWT نتیجه ثبت نام شامل توکن</returns>
        public async Task<ServiceResult<JwtResponseDto>> SignUpAsync(RegisterDto registerDto)
        {
            var serviceResult = new ServiceResult<JwtResponseDto>();

            //try sign up user 
            var result = await _authRepository.SignUpAsync(registerDto);

            if (result.IsSucceeded is false)
            {
                _logger.LogWarning("Registration failed for Email {Email}", registerDto.Email);

                serviceResult.Message = ".ثبت نام انجام نشد";
                serviceResult.Errors = result.Errors;
                serviceResult.StatusCode = ResultStatusCode.BadRequest;

                return serviceResult;
            }

            //generate jwt token
            string token = _jwtTokenGenerator
                .GenerateToken(registerDto.Email, result.UserId, result.Role);

            serviceResult.StatusCode = ResultStatusCode.Success;
            serviceResult.Data = new JwtResponseDto { Token = token };

            _logger.LogInformation("Registration succeeded for Email {Email}", registerDto.Email);

            return serviceResult;
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// برای کاربر موجود، توکن بازیابی رمز عبور ایجاد میکند
        /// </summary>
        /// <param name="forgotPasswordDto">ایمیل کاربر برای بازیابی رمز عبور</param>
        /// <returns>نتیجه شامل توکن بازیابی یا پیام خطا</returns>
        public async Task<ServiceResult<ResetTokenResponseDto>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var serviceResult = new ServiceResult<ResetTokenResponseDto>();

            //is user exist?
            var result = await _authRepository.IsUserExistAsync(forgotPasswordDto.Email);

            if (result is false)
            {
                serviceResult.Message = "با این ایمیل ثبت نامی انجام نشده.";
                serviceResult.StatusCode = ResultStatusCode.NotFound;

                return serviceResult;
            }
            //generate reset passowrd token
            string resetToken = await _authRepository.GeneratePasswordResetTokenAsync(forgotPasswordDto);
            //encoding token
            resetToken = _tokenEncoder.Encode(resetToken);

            serviceResult.StatusCode = ResultStatusCode.Success;
            serviceResult.Data = new ResetTokenResponseDto
            {
                ResetToken = resetToken
            };

            return serviceResult;
        }
        #endregion

        #region ResetPassword
        /// <summary>
        /// با استفاده از توکن بازیابی، رمز عبور کاربر را تغییر میدهد
        /// </summary>
        /// <param name="resetPassword">توکن بازیابی و رمز عبور جدید</param>
        /// <returns>نتیجه تغییر رمز عبور</returns>
        public async Task<ServiceResult<string>> ResetPasswordAsync
            (ResetPasswordDto resetPassword)
        {
            var serviceResult = new ServiceResult<string>();

            //decode token
            resetPassword.ResetToken = _tokenEncoder.Decode(resetPassword.ResetToken);

            //reset password
            var result = await _authRepository.ResetPasswordAsync(resetPassword);

            // check result
            if (result.IsSucceeded is false)
            {
                serviceResult.Message = ".تغییر رمز عبور انجام نشد";
                serviceResult.StatusCode = ResultStatusCode.BadRequest;
                serviceResult.Errors = result.Errors;

                return serviceResult;
            }

            serviceResult.Message = ".رمز عبور با موفقیت تغییر کرد";
            serviceResult.StatusCode = ResultStatusCode.Success;

            return serviceResult;
        }
        #endregion

        #region GetUserRoles
        /// <summary>
        /// فهرست نقش های موجود کاربران را دریافت میکند
        /// </summary>
        /// <returns>نتیجه شامل فهرست نقش ها یا پیام خطا</returns>
        public async Task<ServiceResult<IEnumerable<UserRolesDto>>> GetUserRolesAsync()
        {
            var result = new ServiceResult<IEnumerable<UserRolesDto>>();

            var userRoles = await _authRepository.GetUserRolesAsync();
            if (!userRoles.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".هیچ نقشی یافت نشد";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = userRoles;

            return result;
        }
        #endregion

        #region ChangeUserRole
        /// <summary>
        /// نقش کاربر مشخص شده را تغییر میدهد
        /// </summary>
        /// <param name="roleChange">شناسه کاربر و نقش جدید</param>
        /// <returns>نتیجه تغییر نقش</returns>
        public async Task<ServiceResult<string>> ChangeUserRoleAsync(RoleChangeDto roleChange)
        {
            var serviceResult = new ServiceResult<string>();

            var result = await _authRepository.ChangeUserRoleAsync(roleChange);

            if (!result.IsSucceeded)
            {
                serviceResult.Message = result.Errors.FirstOrDefault();
                serviceResult.StatusCode = ResultStatusCode.NotFound;
                return serviceResult;
            }

            serviceResult.StatusCode = ResultStatusCode.Success;
            serviceResult.Message = ".نقش کاربر با موفقیت تغییر کرد";

            return serviceResult;
        }
        #endregion
    }
}
