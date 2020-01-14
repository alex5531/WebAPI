using AutoMapper;
using WebAPI.BLL.Models;
using WebAPI.DAL.MSSQL.Models;

namespace WebAPI.BLL.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserEntity>();
            CreateMap<UserEntity, User>();
        }
    }
}
