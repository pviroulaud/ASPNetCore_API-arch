using AutoMapper;
using usersDTO;
using userEntities.userModel;

namespace users.API.Profiles
{
    public class mappers : Profile
    {
        public mappers()
        {
            CreateMap<userDTO, user>();

            CreateMap<user, userDTO>();
        }
    }
}
