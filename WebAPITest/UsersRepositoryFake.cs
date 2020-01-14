using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebAPI.DAL.MSSQL.Contracts;
using WebAPI.DAL.MSSQL.Models;
using System.Linq;

namespace WebAPI.DAL.MSSQL
{
	public class UsersRepositoryFake : IUsersRepository
	{
		private readonly IOptionsMonitor<UsersMSSQLRepositoryOption> _options;
		internal readonly static List<UserEntity> _userEntities = new List<UserEntity>()
		{
			 new UserEntity() { Id = 1, DoB = new DateTime(2001, 01, 01), Email = "test1@test.com", FirstName = "FirstName1", LastName = "LastName1", Password = "Password1" },
			 new UserEntity() { Id = 2, DoB = new DateTime(2002, 02, 02), Email = "test2@test.com", FirstName = "FirstName2", LastName = "LastName2", Password = "Password2" },
			 new UserEntity() { Id = 3, DoB = new DateTime(2003, 03, 03), Email = "test3@test.com", FirstName = "FirstName3", LastName = "LastName3", Password = "Password3" },
			 new UserEntity() { Id = 4, DoB = new DateTime(2004, 04, 04), Email = "test4@test.com", FirstName = "FirstName4", LastName = "LastName4", Password = "Password4" },
			 new UserEntity() { Id = 5, DoB = new DateTime(2005, 05, 05), Email = "test5@test.com", FirstName = "FirstName5", LastName = "LastName5", Password = "Password5" }
		};
		public UsersRepositoryFake(IOptionsMonitor<UsersMSSQLRepositoryOption> options)
		{
			_options = options;
		}
		public async Task<UserEntity> CreateUserAsync(UserEntity newUser)
		{
			return await Task.FromResult(
				new UserEntity()
				{
					Id = 1,
					FirstName = newUser.FirstName,
					LastName = newUser.LastName,
					Email = newUser.Email,
					Password = newUser.Password,
					DoB = newUser.DoB
				});
		}

		public async Task<UserEntity> GetUserAsync(int id)
		{
			return await Task.FromResult(_userEntities.FirstOrDefault(x => x.Id == id));
		}

		public async Task<UserEntity> GetUserAsync(string email)
		{
			return await Task.FromResult(_userEntities.FirstOrDefault(x => x.Email.Equals(email)));
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
			var user = _userEntities.FirstOrDefault(x => x.Id.Equals(id));
			if (user == null)
			{
				return await Task.FromResult(false);
			}
			_userEntities.Remove(user);
			return await Task.FromResult(true);
		}

		public async Task<IEnumerable<UserEntity>> GetUsersListAsync(int pageNumber, int pageSize)
		{
			return await Task.FromResult(_userEntities);
		}
	}
}
