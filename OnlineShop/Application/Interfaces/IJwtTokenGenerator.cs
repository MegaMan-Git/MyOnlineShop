using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(string email);
    }
}
