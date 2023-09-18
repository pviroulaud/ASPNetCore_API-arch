using AutoMapper;
using fileEntities.fileModel;
using fileDTO;

namespace log.API.Profiles
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
