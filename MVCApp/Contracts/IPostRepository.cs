using Microsoft.AspNetCore.Http;
using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCApp.Contracts
{
    public interface IPostRepository
    {
        Task<List<PostViewModel>> GetAllAsync();
        Task<HttpResponseMessage> AddPost(Post postView, string token);
        Task<HttpResponseMessage> EditPost(Post postView, string token);
        Task<HttpResponseMessage> DeletePost(int id, string token);
        Task<Post> GetPost(int Id);
    }
}