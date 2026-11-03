using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// مسئول کدگذاری و کدگشایی توکن های مربوط به فرایند تعویض رمز عبور است
    /// </summary>
    public class TokenEncoder : ITokenEncoder
    {
        /// <summary>
        /// کدگذاری میکند Base64Url توکن را با قالب
        /// </summary>
        /// <param name="token">
        /// رشته حاوی توکن
        /// </param>
        /// <returns>رشته کدگذاری شده</returns>
        public string Encode(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        /// <summary>
        /// را به مقدار اصلی تبدیل میکند Base64Url توکن کدگذاری شده با
        /// </summary>
        /// <param name="token">
        /// Base64Url رشته کدگذاری شده با
        /// </param>
        /// <returns>رشته اصلی توکن</returns>
        public string Decode(string token)
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
    }
}
