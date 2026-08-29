using Application.Dtos.Auth;
using Application.Interfaces.Repositories;
using Domain.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        #region DI
        private UserManager<ApplicationUser> _userManager;
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

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }
        #endregion

        #region Reset password
        public async Task<bool> ResetPasswordAsync(string token, string email, RestPasswordDto restPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, restPassword.Password);

            return result.Succeeded;
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
            Result result = new Result();

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
