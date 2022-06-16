using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Queries.Spectacle;
using System;
using System.Collections.Generic;

namespace BoxOffice.Core.MediatR.Queries.Ticket
{
    public class GetAllTicketsQuery : ICacheableMediatrQuery<List<TicketDto>>
    {
        public bool BypassCache { get; set; }
        public string CacheKey => $"TicketList";
        public TimeSpan? SlidingExpiration { get; set; }
    }
}
