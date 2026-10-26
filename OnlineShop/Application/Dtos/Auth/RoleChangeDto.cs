using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Auth
{
    public class RoleChangeDto
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}