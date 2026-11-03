using Application.Dtos.Auth;
using Application.Interfaces.Repositories;
using Domain.Common;
using Infrastructure.Identity;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Identity مربوط به مدیریت کاربران، احراز هویت و عملیات مرتبط با Repository
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        #region DI 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopContext _context;
        /// <summary>
        /// دریافت میکند Dependency Injection را از طریق Repository وابستگی های مورد نیاز
        /// </summary>
        public AuthRepository(UserManager<ApplicationUser> userManager,
            OnlineShopContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region IsUserExist
        /// <summary>
        /// بررسی وجود کاربر بر اساس ایمیل
        /// </summary>
        /// <param name="email">ایمیل ارسال شده جهت پیدا کردن کاربر</param>
        /// <returns>اگر کاربر پیدا شود درست و اگر نشود نادرست</returns>
        public async Task<bool> IsUserExistAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            return (result is not null);
        }
        #endregion

        #region GeneratePasswordResetToken
        /// <summary>
        /// ایجاد توکن جدید برای کاربر در صورت فراموشی رمز عبور
        /// </summary>
        /// <param name="forgotPassword">
        /// شامل ایمیل کاربر Dto
        /// </param>
        /// <returns>
        /// یک توکن در صورت وجود ایمیل یا رشته ای خالی در صورت وجود نداشتن کاربر
        /// </returns>
        public async Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user is null)
            {
                return string.Empty;
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        #endregion

        #region Reset password
        /// <summary>
        /// تغییر رمز عبور در صورت ارسال توکن از قبل ساخته شده توسط درخواست فراموشی رمز عبور
        /// </summary>
        /// <param name="resetPassword">
        /// اطلاعات مورد نیاز ریست شامل ایمیل، توکن و رمز جدید
        /// </param>
        /// <returns>نتیجه ی درخواست و خطاهای آن در صورت عدم موفقیت</returns>
        public async Task<Result> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var result = new Result();

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user is null)
            {
                result.IsSucceeded = false;
                result.Errors.Add(".آدرس ایمیل صحیح نمیباشد");

                return result;
            }

            var identityResult = await _userManager
                .ResetPasswordAsync(user, resetPassword.ResetToken, resetPassword.Password);

            if (identityResult.Succeeded is false)
            {
                result.IsSucceeded = false;
                result.Errors.AddRange(
                    identityResult.Errors.Select(x => x.Description)); 

                return result;
            }

            result.IsSucceeded = true;
            return result;
        }
        #endregion

        #region SignIn
        /// <summary>
        /// با استفاده از ایمیل و رمز عبور کاربر را احراز هویت میکند
        /// </summary>
        /// <param name="login">اطلاعات ورود شامل: ایمیل و رمز عبور</param>
        /// <returns>نتیجه احراز هویت به همراه شناسه و نقش کاربر</returns>
        public async Task<Result> SignInAsync(LoginDto login)
        {
            var result = new Result();

            //Find User
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                result.IsSucceeded = false;
                result.Errors.Add(".کاربری با این مشخصات یافت نشد");
                return result;
            }

            //Check Password
            result.IsSucceeded = await _userManager.CheckPasswordAsync(user, login.Password);
            if (result.IsSucceeded is false)
            {
                result.Errors.Add(".کاربری با این مشخصات یافت نشد");
                return result;
            }

            result.UserId = user.Id;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                result.IsSucceeded = false;
                result.Errors.Add(".کاربر هیچ نقشی ندارد");
                return result;
            }

            result.Role = roles[0];
            return result;
        }
        #endregion

        #region SignUp
        /// <summary>
        /// با دریافت اطلاعات مورد نیاز توسط پارامتر ورودی کاربر را ثبت نام میکند
        /// </summary>
        /// <param name="register">اطلاعات ثبت نام شامل: نام کاربری، ایمیل، رمز عبور و شماره همراه</param>
        /// <returns>نتیجه ثبت نام به همراه شناسه و نقش کاربر</returns>
        public async Task<Result> SignUpAsync(RegisterDto register)
        {
            var result = new Result();

            //create object
            var user = new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber
            };

            //create user
            var identityResult = await _userManager.CreateAsync(user, register.Password);

            //return result
            if (!identityResult.Succeeded)
            {
                result.IsSucceeded = false;
                result.Errors.AddRange(
                    identityResult.Errors.Select(e => e.Description));

                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded)
            {
                result.IsSucceeded = false;
                result.Errors.AddRange(
                    roleResult.Errors.Select(e => e.Description));

                await _userManager.DeleteAsync(user);
                return result;
            }

            result.UserId = user.Id;
            result.Role = "Customer";
            result.IsSucceeded = true;

            return result;
        }
        #endregion

        #region GetUserRoles
        /// <summary>
        /// دریافت نقش تمام کاربران موجود
        /// </summary>
        /// <returns>نتیجه شامل فهرست نقش ها یا پیام خطا</returns>
        public async Task<IEnumerable<UserRolesDto>> GetUserRolesAsync()
        {
            return await (
                from users in _context.Users
                join userroles in _context.UserRoles
                    on users.Id equals userroles.UserId
                join roles in _context.Roles
                    on userroles.RoleId equals roles.Id
                select new UserRolesDto
                {
                    Email = users.Email,
                    Role = roles.Name
                }).AsNoTracking().ToListAsync();
        }
        #endregion

        #region ChangeUserRole
        /// <summary>
        /// تغییر نقش کاربر توسط ادمین
        /// </summary>
        /// <param name="roleChange">اطلاعاتی شامل ایمیل و نقش جدید</param>
        /// <returns>نتیجه درخواست به همراه خطاها در صورت عدم موفقیت</returns>
        public async Task<Result> ChangeUserRoleAsync(RoleChangeDto roleChange)
        {
            var result = new Result();

            var user = await _userManager.FindByEmailAsync(roleChange.Email);

            if (user is null)
            {
                result.IsSucceeded = false;
                result.Errors.Add(".ایمیلی به این آدرس ثبت نشده است");

                return result;
            }

            //get role
            var currentRoles = await _userManager.GetRolesAsync(user);

            var identitiyResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (identitiyResult.Succeeded is false)
            {
                result.IsSucceeded = false;
                result.Errors.AddRange(
                    identitiyResult.Errors.Select(e => e.Description));

                return result;
            }

            //delete currentrole then add new role 
            identitiyResult = await _userManager.AddToRoleAsync(user, roleChange.Role);
            if (identitiyResult.Succeeded is false)
            {
                result.IsSucceeded = false;
                result.Errors.AddRange(
                    identitiyResult.Errors.Select(e => e.Description));

                return result;
            }

            result.IsSucceeded = true;

            return result;
        }
        #endregion
    }
}
