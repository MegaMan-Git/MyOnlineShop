namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل دریافت ایمیل برای بازیابی رمز عبور
    /// </summary>
    public class ForgotPasswordDto
    {
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
}
