using BoxOffice.Core.Dto;
using MediatR;

namespace BoxOffice.Core.MediatR.Commands.Spectacle
{
    public class UpdateSpectacleCommand : IRequest<SpectacleDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint TotalTicket { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
    }
}
