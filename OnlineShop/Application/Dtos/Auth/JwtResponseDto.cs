namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل پاسخ شامل توکن احراز هویت
    /// </summary>
    public class JwtResponseDto
    {
        /// <summary>
        /// توکن احراز هویت
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
