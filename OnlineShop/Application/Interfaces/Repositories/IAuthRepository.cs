using Application.Dtos.Auth;
using Domain.Common;

namespace Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        /// <summary>
        /// بررسی وجود کاربر بر اساس ایمیل
        /// </summary>
        /// <param name="email">ایمیل ارسال شده جهت پیدا کردن کاربر</param>
        /// <returns>اگر کاربر پیدا شود درست و اگر نشود نادرست</returns>
        Task<bool> IsUserExistAsync(string Email);
        
        /// <summary>
        /// ایجاد توکن جدید برای کاربر در صورت فراموشی رمز عبور
        /// </summary>
        /// <param name="forgotPassword">
        /// شامل ایمیل کاربر Dto
        /// </param>
        /// <returns>
        /// یک توکن در صورت وجود ایمیل یا رشته ای خالی در صورت وجود نداشتن کاربر
        /// </returns>
        Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPassword);
        
        /// <summary>
        /// تغییر رمز عبور در صورت ارسال توکن از قبل ساخته شده توسط درخواست فراموشی رمز عبور
        /// </summary>
        /// <param name="resetPassword">
        /// اطلاعات مورد نیاز ریست شامل ایمیل، توکن و رمز جدید
        /// </param>
        /// <returns>نتیجه ی درخواست و خطاهای آن در صورت عدم موفقیت</returns>
        Task<Result> ResetPasswordAsync(ResetPasswordDto resetPassword);
        
        /// <summary>
        /// با استفاده از ایمیل و رمز عبور کاربر را احراز هویت میکند
        /// </summary>
        /// <param name="login">اطلاعات ورود شامل: ایمیل و رمز عبور</param>
        /// <returns>نتیجه احراز هویت به همراه شناسه و نقش کاربر</returns>
        Task<Result> SignInAsync(LoginDto login);
        
        /// <summary>
        /// با دریافت اطلاعات مورد نیاز توسط پارامتر ورودی کاربر را ثبت نام میکند
        /// </summary>
        /// <param name="register">
        /// اطلاعات ثبت نام شامل: نام کاربری، ایمیل، رمز عبور و شماره همراه
        /// </param>
        /// <returns>نتیجه ثبت نام به همراه شناسه و نقش کاربر</returns>
        Task<Result> SignUpAsync(RegisterDto register);

        /// <summary>
        /// تغییر نقش کاربر توسط ادمین
        /// </summary>
        /// <param name="roleChange">اطلاعاتی شامل ایمیل و نقش جدید</param>
        /// <returns>نتیجه درخواست به همراه خطاها در صورت عدم موفقیت</returns>
        Task<Result> ChangeUserRoleAsync(RoleChangeDto roleChange);
        
        /// <summary>
        /// دریافت نقش تمام کاربران موجود
        /// </summary>
        /// <returns>نتیجه شامل فهرست نقش ها یا پیام خطا</returns>
        Task<IEnumerable<UserRolesDto>> GetUserRolesAsync();
    }
}
