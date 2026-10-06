using Application.Dtos.Auth;
using Application.Interfaces.Repositories;
using Domain.Common;
using Domain.Enums;
using Infrastructure.Identity;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        #region DI
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnlineShopContext _context;
        public AuthRepository(UserManager<ApplicationUser> userManager,
            OnlineShopContext context)
        {
            _userManager = userManager;
            _context = context;
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
                }).ToListAsync();
        }
        #endregion

        #region ChangeUserRole
        public async Task<Result> ChangeUserRoleAsync(RoleChangeDto roleChange)
        {
            var result = new Result();

            var user = await _userManager.FindByEmailAsync(roleChange.Email);

            if (user is null)
            {
                result.IsSucceeded = false;
                result.Errors.Add("ایمیل یافت نشد.");
                return result;
            }

            //get role
            var currentRoles = await _userManager.GetRolesAsync(user);

            //delete currentrole then add new role 
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, roleChange.Role);

            result.IsSucceeded = true;

            return result;
        }
        #endregion
    }
}
