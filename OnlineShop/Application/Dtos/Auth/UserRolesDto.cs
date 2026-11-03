namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل نمایش ایمیل و نقش کاربر
    /// </summary>
    public class UserRolesDto
    {
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// نقش کاربر
        /// </summary>
        public string? Role { get; set; }
    }
}
