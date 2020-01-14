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
    public class PostsService : IPostsService
    {
        private readonly IMapper _mapper;
        public IPostsRepository PostsRepo { get; private set; }

        public PostsService(IMapper mapper, IPostsRepository postsRepo)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            PostsRepo = postsRepo ?? throw new ArgumentNullException(nameof(postsRepo));
        }
        public async Task<Post> CreatePostAsync(Post post)
        {
            var newPost = await PostsRepo.CreatePostAsync(_mapper.Map<PostEntity>(post));
            return _mapper.Map<Post>(newPost);
        }

        public async Task<int> DeletePostAsync(int id, int userId)
        {
            return await PostsRepo.DeletePostAsync(id, userId);
        }

        public async Task<Post> GetPostAsync(int id)
        {
            var post = await PostsRepo.GetPostAsync(id);
            return _mapper.Map<Post>(post);
        }

        public async Task<int> UpdatePostAsync(Post post, int userId)
        {
            return await PostsRepo.UpdatePostAsync(_mapper.Map<PostEntity>(post), userId);
        }

        public async Task<IEnumerable<Post>> GetPostsListAsync(int pageNumber, int pageSize)
        {
            var posts = await PostsRepo.GetPostsListAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<Post>>(posts);
        }
    }
}
