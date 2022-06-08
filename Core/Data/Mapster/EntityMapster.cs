using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using Mapster;

namespace BoxOffice.Core.Data.Mapster
{
    public class EntityMapster
    {
        public TypeAdapterConfig GlobalConfig { get; set; }
        public EntityMapster()
        {
            var conf = new TypeAdapterConfig();

            conf.NewConfig<Ticket, TicketDto>().
                Map(dest => dest.ClientFullName, src => $"{src.Client.FirstName} {src.Client.LastName}").
                IgnoreIf((src, dest) => src.Client == null, dest => dest.ClientFullName).
                Map(dest => dest.SpectacleName, src => src.Spectacle.Name).
                IgnoreIf((src, dest) => src.Spectacle == null, dest => dest.SpectacleName).
                Map(dest => dest.SpectacleStartTime, src => src.Spectacle.StartTime).
                IgnoreIf((src, dest) => src.Spectacle == null, dest => dest.SpectacleStartTime).
                Map(dest => dest.SpectacleEndTime, src => src.Spectacle.EndTime).
                IgnoreIf((src, dest) => src.Spectacle == null, dest => dest.SpectacleEndTime);

            GlobalConfig = conf;
        }
    }
}
//CreateMap<Ticket, TicketDto>()
//              .ForMember(dto => dto.ClientFullName, conf => conf.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"))
//              .ForMember(dto => dto.SpectacleName, conf => conf.MapFrom(src => src.Spectacle.Name))
//              .ForMember(dto => dto.SpectacleStartTime, conf => conf.MapFrom(src => src.Spectacle.StartTime))
//              .ForMember(dto => dto.SpectacleEndTime, conf => conf.MapFrom(src => src.Spectacle.EndTime))
//              ;
