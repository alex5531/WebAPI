using AutoMapper;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class PostViewMapping : Profile
    {
        public PostViewMapping()
        {
            CreateMap<Post, BLL.Models.Post>();
            CreateMap<BLL.Models.Post, Post>();
        }
    }
}
