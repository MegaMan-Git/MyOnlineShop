using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class RestPasswordDto
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
