﻿using AutoMapper;
using logDTO;
using logEntities.logModel;

namespace log.API.Profiles
{
    public class mappers : Profile
    {
        public mappers()
        {
            CreateMap<errorDTO, errorLog>();

            CreateMap<operationDTO, operationLog>();
        }
    }
}
