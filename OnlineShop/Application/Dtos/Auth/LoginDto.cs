using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class LoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
