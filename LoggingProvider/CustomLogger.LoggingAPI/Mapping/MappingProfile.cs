using AutoMapper;
using CustomLogger.Domain.Models;
using CustomLogger.LoggingAPI.Resources;

namespace CustomLogger.LoggingAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLogResource, Log>();
        }
    }
}
