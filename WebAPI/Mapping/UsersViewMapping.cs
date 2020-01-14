using AutoMapper;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class UsersViewMapping : Profile
    {
        public UsersViewMapping()
        {

            CreateMap<BLL.Models.User, User>();
            CreateMap<User, BLL.Models.User>();
        }
    }
}
