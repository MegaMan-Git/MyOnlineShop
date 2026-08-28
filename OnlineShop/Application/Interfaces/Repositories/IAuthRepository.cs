using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        public Task<bool> FindUser();

        public Task<bool> SignIn();

        public Task<string> RestPassword_CreateToken();

        public Task<string> RestPassword();

    }
}
