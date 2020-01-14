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
	public class PostsRepository : IPostsRepository
	{
		private readonly IOptionsMonitor<UsersMSSQLRepositoryOption> _options;

		public PostsRepository(IOptionsMonitor<UsersMSSQLRepositoryOption> options)
		{
			_options = options;
		}
		public async Task<PostEntity> CreatePostAsync(PostEntity newPost)
		{
			const string sqlQuery = @"INSERT INTO Posts (
									UserId,
									Title,
									Body)
									OUTPUT INSERTED.*
									VALUES (
									@UserId,
									@Title,
									@Body);
									SELECT CAST(SCOPE_IDENTITY() as int)";


			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QuerySingleOrDefaultAsync<PostEntity>(sqlQuery, newPost, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<PostEntity> GetPostAsync(int id)
		{
			const string sqlQuery = @"SELECT
								id,
								UserId,
								Title,
								Body
								FROM Posts
								WHERE id=@id;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QueryFirstOrDefaultAsync<PostEntity>(sqlQuery, new { id }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<int> UpdatePostAsync(PostEntity post, int userId)
		{
			const string sqlQuery = @"UPDATE Posts SET 
									Title = @Title,
									Body = @Body
									WHERE id = @id AND UserId = @UserId;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.ExecuteAsync(sqlQuery, new { post.Title, post.Body,		post.Id, UserId = userId }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<int> DeletePostAsync(int id, int userId)
		{
			const string sqlQuery = @"DELETE posts WHERE id=@id AND UserId=@userId;";
			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.ExecuteAsync(sqlQuery, new { id, userId }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}

		public async Task<IEnumerable<PostEntity>> GetPostsListAsync(int pageNumber, int pageSize)
		{
			var offset = pageNumber <= 1 ? 0 : (pageNumber - 1) * pageSize;
			const string sqlQuery = @"SELECT
								id,
								UserId,
								Title,
								Body
								FROM Posts
								ORDER BY id 
								OFFSET @offset ROWS
								FETCH NEXT @pageSize ROWS ONLY;";

			using (var db = new SqlConnection(_options.CurrentValue.UsersDbConnectionString))
			{
				return await db.QueryAsync<PostEntity>(sqlQuery, new { offset, pageSize }, commandType: CommandType.Text).ConfigureAwait(false);
			}
		}
	}
}
