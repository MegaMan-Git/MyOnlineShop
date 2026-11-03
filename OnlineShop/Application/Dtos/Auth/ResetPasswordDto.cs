namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل اطلاعات تغییر رمز عبور
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// توکن بازیابی رمز عبور
        /// </summary>
        public string ResetToken { get; set; } = string.Empty;
        /// <summary>
        /// رمز عبور کاربر
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// تکرار رمز عبور
        /// </summary>
        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
