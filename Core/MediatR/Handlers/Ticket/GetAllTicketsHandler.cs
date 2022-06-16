using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Queries.Ticket;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Ticket
{
    public class GetAllTicketsHandler : IRequestHandler<GetAllTicketsQuery, List<TicketDto>>
    {
        private readonly ITicketService _service;

        public GetAllTicketsHandler(ITicketService service)
        {
            _service = service;
        }

        public Task<List<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            return _service.GetAll();
        }
    }
}
