using Application.Dtos.Auth;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<JwtResponseDto>> SignInAsync(LoginDto loginDto);
        Task<ServiceResult<JwtResponseDto>> SignUpAsync(RegisterDto registerDto);
        Task<ServiceResult<ResetTokenResponseDto>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<ServiceResult<JwtResponseDto>> ResetPasswordAsync(ResetPasswordDto resetPassword);
    }
}
