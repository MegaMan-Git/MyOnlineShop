namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// امضاشده و دارای زمان انقضا ایجاد میکند JWT با اطلاعات کاربر، توکن
        /// </summary>
        /// <param name="email">توکن قرار میگیرد Claim ایمیل کاربر که در</param>
        /// <param name="userId">توکن قرار میگیرد Claim شناسه کاربر که در</param>
        /// <param name="role">توکن قرار میگیرد Claim نقش کاربر که برای احراز مجوز در</param>
        /// <returns>JWT رشته حاوی توکن</returns>
        public string GenerateToken(string email, string userId, string role);
    }
}
