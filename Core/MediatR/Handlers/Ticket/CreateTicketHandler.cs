using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Commands.Ticket;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Ticket
{
    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, TicketDto>
    {
        private readonly ITicketService _service;

        public CreateTicketHandler(ITicketService service)
        {
            _service = service;
        }

        public Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            return _service.BuyAsync(request, request.Client);
        }
    }
}
