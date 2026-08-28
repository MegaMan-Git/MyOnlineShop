using Application.Interfaces.Repositories;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private UserManager<ApplicationUser> _userManager;
        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<bool> FindUser()
        {
            throw new NotImplementedException();
        }

        public Task<string> RestPassword()
        {
            throw new NotImplementedException();
        }

        public Task<string> RestPassword_CreateToken()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignIn()
        {
            throw new NotImplementedException();
        }
    }
}
