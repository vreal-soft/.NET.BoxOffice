using System.Collections.Generic;
using BoxOffice.Core.Data.Entities;

namespace BoxOffice.Core.Data.Entities
{
    public partial class SpectacleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint TotalTicket { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
        public IList<Ticket> Tickets { get; set; }
    }
}