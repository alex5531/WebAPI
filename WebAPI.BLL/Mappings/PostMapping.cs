using AutoMapper;
using WebAPI.BLL.Models;
using WebAPI.DAL.MSSQL.Models;

namespace WebAPI.BLL.Mappings
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostEntity>();
            CreateMap<PostEntity, Post>();
        }
    }
}
