using AutoMapper;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;

namespace BoxOffice.Core.Data.Mapper
{
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            CreateMap<Spectacle, SpectacleDto>().ReverseMap();
            CreateMap<Spectacle, CreateSpectacle>().ReverseMap();

            CreateMap<Admin, Registration>().ReverseMap();
            CreateMap<Client, Registration>().ReverseMap();

            CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
