using AutoMapper;
using fileEntities.fileModel;
using fileDTO;

namespace fileAPI.Profiles
{
    public class mappers : Profile
    {
        public mappers()
        {
            CreateMap<userFileDTO, userFile>();
            CreateMap<userFile, userFileDTO>();
        }
    }
}
