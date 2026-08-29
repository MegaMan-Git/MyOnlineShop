using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
