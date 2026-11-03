namespace Application.Dtos.Auth
{
    /// <summary>
    /// مدل اطلاعات ثبت نام کاربر
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// ایمیل کاربر
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// رمز عبور کاربر
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// شماره تلفن کاربر
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
