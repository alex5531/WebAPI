using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebAPI.DAL.MSSQL.Contracts;
using WebAPI.DAL.MSSQL.Models;

namespace WebAPI.DAL.MSSQL
{
	public class UsersRepository : IUsersRepository
	{
		private readonly IOptionsMonitor<UsersMSSQLRepositoryOption> _options;

		public UsersRepository(IOptionsMonitor<UsersMSSQLRepositoryOption> options)
		{
			_options = options;
		}
		public async Task<UserEntity> CreateUserAsync(UserEntity newUser)
		{
			const string sqlQuery = @"INSERT INTO Users (
									FirstName,
									LastName,
									Email,
									Password,
									DoB)
									OUTPUT INSERTED.*
									VALUES (
									@FirstName,
									@LastName,
									@Email,
									@Password,
									@DoB);
									SELECT CAST(SCOPE_IDENTITY() as int)";


			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QuerySingleOrDefaultAsync<UserEntity>(sqlQuery, newUser, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<UserEntity> GetUserAsync(int id)
		{
			const string sqlQuery = @"SELECT
								id,
								FirstName,
								LastName,
								Email,
								Password,
								DoB
								FROM Users
								WHERE id=@id;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QueryFirstOrDefaultAsync<UserEntity>(sqlQuery, new { id }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<UserEntity> GetUserAsync(string email)
		{
			const string sqlQuery = @"SELECT
								id,
								FirstName,
								LastName,
								Email,
								Password,
								DoB
								FROM Users
								WHERE email=@email;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QueryFirstOrDefaultAsync<UserEntity>(sqlQuery, new { email }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<bool> UpdateUserAsync(UserEntity user)
		{
			const string sqlQuery = @"UPDATE Users SET 
									FirstName = @FirstName,
									LastName = @LastName,
									Email = @Email,
									Password = @Password,
									DoB = @Dob
									WHERE id = @id;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				await db.ExecuteAsync(sqlQuery, user, commandType: CommandType.Text).ConfigureAwait(false);
				return true;
			}
		}

		public async Task<bool> DeleteUserAsync(int id)
		{
			const string sqlQuery = @"DELETE Users WHERE id=@id;";
			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				await db.ExecuteAsync(sqlQuery, new { id = id }, commandType: CommandType.Text).ConfigureAwait(false);
				return true;
			}
		}

		public async Task<IEnumerable<UserEntity>> GetUsersListAsync(int pageNumber, int pageSize)
		{
			var offset = pageNumber <= 1 ? 0 : (pageNumber - 1) * pageSize;
			const string sqlQuery = @"SELECT
								id,
								FirstName,
								LastName,
								Email,
								[Password],
								DoB
								FROM Users
								ORDER BY id 
								OFFSET @offset ROWS
								FETCH NEXT @pageSize ROWS ONLY;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QueryAsync<UserEntity>(sqlQuery, new { offset, pageSize }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}
	}
}
