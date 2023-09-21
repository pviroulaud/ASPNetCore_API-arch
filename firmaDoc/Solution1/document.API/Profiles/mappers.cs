using AutoMapper;
using documentDTO;
using documentEntities.documentModel;

namespace documentAPI.Profiles
{
    public class mappers:Profile
    {
        public mappers()
        {
            CreateMap<document, userDocumentDTO>();

            CreateMap<userDocumentDTO, document>();
        }
    }
}
