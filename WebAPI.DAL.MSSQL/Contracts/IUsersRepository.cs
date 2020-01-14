using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.DAL.MSSQL.Models;

namespace WebAPI.DAL.MSSQL.Contracts
{
    public interface IUsersRepository
    {
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task<UserEntity> GetUserAsync(int id);
        Task<UserEntity> GetUserAsync(string email);
        Task<bool> UpdateUserAsync(UserEntity user);
        Task<bool> DeleteUserAsync(int id);

        Task<IEnumerable<UserEntity>> GetUsersListAsync(int pageNumber, int pageSize);
    }
}
