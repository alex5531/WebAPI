using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.BLL.Models;

namespace WebAPI.BLL.Contracts
{
    public interface IPostsService
    {
        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<Post> CreatePostAsync(Post post);

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Post> GetPostAsync(int id);

        /// <summary>
        /// Update post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<int> UpdatePostAsync(Post post, int userId);

        /// <summary>
        /// Delete post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeletePostAsync(int id, int userId);

        /// <summary>
        /// Get post list 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> GetPostsListAsync(int pageNumber, int pageSize);
    }
}
