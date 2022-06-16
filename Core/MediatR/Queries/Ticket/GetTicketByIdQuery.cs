using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Queries.Spectacle;
using System;

namespace BoxOffice.Core.MediatR.Queries.Ticket
{
    public class GetTicketByIdQuery : ICacheableMediatrQuery<TicketDto>
    {
        public int Id { get; set; }
        public bool BypassCache { get; set; }
        public string CacheKey => $"Ticket-{Id}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetTicketByIdQuery(int id)
        {
            Id = id;
        }
    }
}
