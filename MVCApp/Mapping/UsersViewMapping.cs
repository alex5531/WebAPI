using AutoMapper;
using MVCApp.Models;

namespace MVCApp.Mapping
{
    public class UsersViewMapping : Profile
    {
        public UsersViewMapping()
        {
            CreateMap<UserView, User>();
            CreateMap<User, UserView>();
        }
    }
}