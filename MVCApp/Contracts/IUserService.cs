using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;
using System;
using System.Threading.Tasks;

namespace MVCApp.Contracts
{
    public interface IUserService
    {
        public Task<string> SignUpAsync(UserView user);
        public Task<Tuple<string, string>> GenerateToken(LoginViewModel user);
    }
}