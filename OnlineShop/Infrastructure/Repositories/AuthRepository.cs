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
            Result result = new();

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user is null)
            {
                result.IsSucceeded = false;
                result.Errors.Add("آدرس ایمیل صحیح نمیباشد.");

                return result;
            }
            
            var identityResult = await _userManager
                .ResetPasswordAsync(user, resetPassword.ResetToken, resetPassword.Password);

            if(identityResult.Succeeded is false)
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
        public async Task<bool> SignInAsync(LoginDto login)
        {
            //Find User
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                return false;
            }

            //Check Password
            var result = await _userManager.CheckPasswordAsync(user, login.Password);

            return result;
        }
        #endregion

        #region SignUp
        public async Task<Result> SignUpAsync(RegisterDto register)
        {
            Result result = new();

            //Create User
            var identityResult = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
            }, register.Password);

            //return result
            if (identityResult.Succeeded)
            {
                result.IsSucceeded = true;
                return result;
            }
            else
            {
                result.IsSucceeded = false;
                foreach (var err in identityResult.Errors)
                {
                    result.Errors.Add(err.Description);
                }

                return result;
            }

        }
        #endregion
    }
}
