using AutoMapper;
using MVCApp.Models;

namespace MVCApp.Mapping
{
    public class PostViewMapping : Profile
    {
        public PostViewMapping()
        {
            CreateMap<Post, PostViewModel>();
            CreateMap<PostViewModel, Post>();
        }
    }
}