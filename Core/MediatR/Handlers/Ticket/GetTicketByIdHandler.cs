using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Queries.Ticket;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Ticket
{
    public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdQuery, TicketDto>
    {
        public ITicketService _serivce { get; set; }

        public GetTicketByIdHandler(ITicketService serivce)
        {
            _serivce = serivce;
        }

        public Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            return _serivce.GetById(request.Id);
        }
    }
}
