using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using MediatR;

namespace BoxOffice.Core.Commands
{
    public class CreateSpectacleCommand : IRequest<SpectacleDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint TotalTicket { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
        public Admin admin;
    }
}
