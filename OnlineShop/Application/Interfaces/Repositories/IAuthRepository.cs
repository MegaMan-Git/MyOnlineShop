using Application.Dtos.Auth;
using Domain.Common;

namespace Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> IsUserExistAsync(string Email);
        Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPassword);
        Task<bool> ResetPasswordAsync(string token, string email, RestPasswordDto restPassword);
        Task<bool> SignInAsync(LoginDto login);
        Task<Result> SignUpAsync(RegisterDto register);    
    }
}
