namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل اطلاعات ورود کاربر
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// رمز عبور کاربر
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
