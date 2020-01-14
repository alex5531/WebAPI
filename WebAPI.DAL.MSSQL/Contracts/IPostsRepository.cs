using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL.MSSQL.Models;

namespace WebAPI.DAL.MSSQL.Contracts
{
    public interface IPostsRepository
    {
        Task<PostEntity> CreatePostAsync(PostEntity post);
        Task<PostEntity> GetPostAsync(int id);
        Task<int> UpdatePostAsync(PostEntity post, int userId);
        Task<int> DeletePostAsync(int id, int userId);

        Task<IEnumerable<PostEntity>> GetPostsListAsync(int pageNumber, int pageSize);
    }
}
