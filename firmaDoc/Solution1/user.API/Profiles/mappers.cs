using AutoMapper;
using usersDTO;
using userEntities.userModel;

namespace usersAPI.Profiles
{
    public class mappers : Profile
    {
        public mappers()
        {
            CreateMap<userDTO, user>();
            CreateMap<user, userDTO>();

            CreateMap<newUserDTO, user>();
            CreateMap<user, userWithPermitDTO>();

            CreateMap<permit, permitDTO>();
            CreateMap<permitDTO, permit>();

            CreateMap<userPermits, userPermitDTO>();
            CreateMap<userPermitDTO, userPermits>();
        }
    }
}
