using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.BLL.Contracts;
using WebAPI.BLL.Models;
using WebAPI.DAL.MSSQL.Contracts;
using WebAPI.DAL.MSSQL.Models;

namespace WebAPI.BLL
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordStorage _passwordStorage;
        public IUsersRepository UsersRepo { get; private set; }

        public UsersService(IMapper mapper, IUsersRepository usersRepo, IPasswordStorage passwordStorage)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            UsersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
            _passwordStorage = passwordStorage ?? throw new ArgumentNullException(nameof(passwordStorage));
        }
        public async Task<User> CreateUserAsync(User user)
        {
            user.Password = _passwordStorage.CreateHash(user.Password); 
            var newUser = await UsersRepo.CreateUserAsync(_mapper.Map<UserEntity>(user));
            return _mapper.Map<User>(newUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await UsersRepo.DeleteUserAsync(id);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await UsersRepo.GetUserAsync(id);
            return _mapper.Map<User>(user);
        }
        public async Task<User> GetUserAsync(string email)
        {
            var user = await UsersRepo.GetUserAsync(email);
            return _mapper.Map<User>(user);
        }

        public async Task<IEnumerable<User>> GetUsersListAsync(int pageNumber, int pageSize)
        {
            var users = await UsersRepo.GetUsersListAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await UsersRepo.UpdateUserAsync(_mapper.Map<UserEntity>(user));
        }

        public async Task<int> ValidateUserAsync(string email, string password)
        {
            var user = await GetUserAsync(email);
            if (user != null)
            {
                return _passwordStorage.VerifyPassword(password, user.Password) ? user.Id : int.MinValue;
            }
            return int.MinValue;
        }
    }
}
