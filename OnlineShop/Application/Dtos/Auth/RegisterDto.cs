using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email {  get; set; } =string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
