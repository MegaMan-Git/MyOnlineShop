using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class RegisterDto
    {
        public string? UserName {  get; set; }
        public string? Email {  get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber {  get; set; }
    }
}
