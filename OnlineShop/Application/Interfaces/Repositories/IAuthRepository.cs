using Application.Dtos.Auth;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> IsUserExistAsync(string Email);

        Task<Result> SignUpAsync(RegisterDto register);

        Task<bool> SignInAsync(LoginDto login);
        
        Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPassword);
        
        Task<bool> ResetPasswordAsync(string token,string email,RestPasswordDto restPassword);

    }
}
