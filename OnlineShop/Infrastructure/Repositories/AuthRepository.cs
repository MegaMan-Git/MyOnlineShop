using Application.Dtos.Auth;
using Application.Interfaces.Repositories;
using Domain.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        #region DI
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region IsUserExist
        public async Task<bool> IsUserExistAsync(string Email)
        {
            var result = await _userManager.FindByEmailAsync(Email);

            return (result is not null);
        }
        #endregion

        #region GeneratePasswordResetToken
        public async Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
            {
                return string.Empty;
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        #endregion

        #region Reset password
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
                foreach (var err in identityResult.Errors)
                {
                    result.Errors.Add(err.Description);
                }

                return result;
            }

            result.IsSucceeded = true;
            return result;
        }
        #endregion

        #region SignIn
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
            return result;
        }
        #endregion

        #region SignUp
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
            if (identityResult.Succeeded)
            {
                result.UserId = user.Id;
                result.IsSucceeded = true;
                return result;
            }
            
            result.IsSucceeded = false;
            foreach (var err in identityResult.Errors)
            {
                result.Errors.Add(err.Description);
            }

            return result;
        }
        #endregion
    }
}
