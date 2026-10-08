using Application.Dtos.Auth;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        #region DI
        private readonly IAuthRepository _authRepository;
        private readonly ITokenEncoder _tokenEncoder;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IAuthRepository authRepository,
            ITokenEncoder tokenEncoder,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;
            _tokenEncoder = tokenEncoder;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        #endregion

        #region SignIn
        public async Task<ServiceResult<JwtResponseDto>> SignInAsync(LoginDto loginDto)
        {
            var serviceResult = new ServiceResult<JwtResponseDto>();

            //find user
            var result = await _authRepository.SignInAsync(loginDto);

            if (result.IsSucceeded is false)
            {
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

            return serviceResult;
        }
        #endregion

        #region SignUp
        public async Task<ServiceResult<JwtResponseDto>> SignUpAsync(RegisterDto registerDto)
        {
            var serviceResult = new ServiceResult<JwtResponseDto>();

            //try sign up user 
            var result = await _authRepository.SignUpAsync(registerDto);

            if (result.IsSucceeded is false)
            {
                serviceResult.Message = ".ثبت نام انجام نشد";
                serviceResult.Errors = result.Errors;
                serviceResult.StatusCode = ResultStatusCode.BadRequest;

                return serviceResult;
            }

            //generate jwt token
            string token = _jwtTokenGenerator.GenerateToken(registerDto.Email, result.UserId, result.Role);

            serviceResult.StatusCode = ResultStatusCode.Success;
            serviceResult.Data = new JwtResponseDto { Token = token };

            return serviceResult;
        }
        #endregion

        #region ForgotPassword
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
        public async Task<ServiceResult<JwtResponseDto>> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var serviceResult = new ServiceResult<JwtResponseDto>();

            //decode token
            resetPassword.ResetToken = _tokenEncoder.Decode(resetPassword.ResetToken);

            //reset password
            var result = await _authRepository.ResetPasswordAsync(resetPassword);
           
            // check result
            if(result.IsSucceeded is false)
            {
                serviceResult.Message = "تغییر رمز عبور انجام نشد.";
                serviceResult.StatusCode = ResultStatusCode.BadRequest;
                serviceResult.Errors = result.Errors;
            
                return serviceResult;
            }

            serviceResult.Message = "رمز عبور با موفقیت تغییر کرد.";
            serviceResult.StatusCode = ResultStatusCode.Success;

            return serviceResult;
        }
        #endregion

        #region GetUserRoles
        public async Task<ServiceResult<IEnumerable<UserRolesDto>>> GetUserRolesAsync()
        {
            var result = new ServiceResult<IEnumerable<UserRolesDto>>();

            var userRoles = await _authRepository.GetUserRolesAsync();
            if (!userRoles.Any())
            {
                result.StatusCode = ResultStatusCode.NotFound;
                result.Message = ".هیچ رولی یافت نشد";

                return result;
            }

            result.StatusCode = ResultStatusCode.Success;
            result.Data = userRoles;

            return result;
        }
        #endregion

        #region ChangeUserRole
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
