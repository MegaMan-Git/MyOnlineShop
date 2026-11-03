namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل تغییر نقش کاربر
    /// </summary>
    public class RoleChangeDto
    {
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// نقش کاربر
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
