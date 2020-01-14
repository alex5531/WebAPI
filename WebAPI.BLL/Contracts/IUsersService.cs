using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.BLL.Models;

namespace WebAPI.BLL.Contracts
{
    public interface IUsersService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUserAsync(int id);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User> GetUserAsync(string email);

        /// <summary>
        /// Update user parameters
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdateUserAsync(User user);

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(int id);

        /// <summary>
        /// Get user list 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersListAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Validates user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<int> ValidateUserAsync(string email, string password);
    }
}
