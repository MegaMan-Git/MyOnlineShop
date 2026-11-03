namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل پاسخ شامل توکن بازیابی رمز عبور
    /// </summary>
    public class ResetTokenResponseDto
    {
        /// <summary>
        /// توکن بازیابی رمز عبور
        /// </summary>
        public string ResetToken { get; set; } = string.Empty;
    }
}
