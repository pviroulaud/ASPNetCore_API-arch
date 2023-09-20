using AutoMapper;
using certificateDTO;
using certificateEntities.certificateModel;

namespace certificateAPI.Profiles
{
    public class mappers:Profile
    {
        public mappers()
        {
            CreateMap<electronicCertificate, readElectronicCertificateDTO>();
            
            CreateMap<electronicCertificate, electronicCertificateDTO>();
        }
    }
}
