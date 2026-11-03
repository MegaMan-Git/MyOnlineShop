using Application.Dtos.Auth;
using Domain.Common;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// ایجاد میکند JWT پس از ورود موفق
        /// </summary>
        /// <param name="loginDto">اطلاعات ورود کاربر</param>
        /// <returns>یا پیام خطا JWT نتیجه ورود شامل توکن</returns>
        Task<ServiceResult<JwtResponseDto>> SignInAsync(LoginDto loginDto);
        
        /// <summary>
        /// ایجاد میکند JWT کاربر جدید را ثبت نام کرده و در صورت موفقیت توکن
        /// </summary>
        /// <param name="registerDto">اطلاعات ثبت نام کاربر</param>
        /// <returns>یا پیام خطا JWT نتیجه ثبت نام شامل توکن</returns>
        Task<ServiceResult<JwtResponseDto>> SignUpAsync(RegisterDto registerDto);
        
        /// <summary>
        /// برای کاربر موجود، توکن بازیابی رمز عبور ایجاد میکند
        /// </summary>
        /// <param name="forgotPasswordDto">ایمیل کاربر برای بازیابی رمز عبور</param>
        /// <returns>نتیجه شامل توکن بازیابی یا پیام خطا</returns>
        Task<ServiceResult<ResetTokenResponseDto>> ForgotPasswordAsync
            (ForgotPasswordDto forgotPasswordDto);
        
        /// <summary>
        /// با استفاده از توکن بازیابی، رمز عبور کاربر را تغییر میدهد
        /// </summary>
        /// <param name="resetPassword">توکن بازیابی و رمز عبور جدید</param>
        /// <returns>نتیجه تغییر رمز عبور</returns>
        Task<ServiceResult<string>> ResetPasswordAsync(ResetPasswordDto resetPassword);
        
        /// <summary>
        /// نقش کاربر مشخص شده را تغییر میدهد
        /// </summary>
        /// <param name="roleChange">شناسه کاربر و نقش جدید</param>
        /// <returns>نتیجه تغییر نقش</returns>
        Task<ServiceResult<string>> ChangeUserRoleAsync(RoleChangeDto roleChange);
        
        /// <summary>
        /// فهرست نقش های موجود کاربران را دریافت میکند
        /// </summary>
        /// <returns>نتیجه شامل فهرست نقش ها یا پیام خطا</returns>
        Task<ServiceResult<IEnumerable<UserRolesDto>>> GetUserRolesAsync();
    }
}
