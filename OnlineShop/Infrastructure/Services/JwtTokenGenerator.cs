using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// است JWT مسئول ایجاد توکن های احراز هویت مبتنی بر
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// تنظیمات مورد نیاز برای ایجاد توکن را از طریق تزریق وابستگی دریافت میکند
        /// </summary>
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// امضاشده و دارای زمان انقضا ایجاد میکند JWT با اطلاعات کاربر، توکن
        /// </summary>
        /// <param name="email">توکن قرار میگیرد Claim ایمیل کاربر که در</param>
        /// <param name="userId">توکن قرار میگیرد Claim شناسه کاربر که در</param>
        /// <param name="role">توکن قرار میگیرد Claim نقش کاربر که برای احراز مجوز در</param>
        /// <returns>JWT رشته حاوی توکن</returns>
        public string GenerateToken(string email, string userId, string role)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claim = new()
            {
               new(ClaimTypes.Email, email),
               new(ClaimTypes.NameIdentifier, userId),
               new(ClaimTypes.Role, role)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claim, DateTime.UtcNow,
                DateTime.UtcNow.AddHours(
                    _configuration.GetValue<int>("Authentication:TokenTimeExpires")
                    ),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
