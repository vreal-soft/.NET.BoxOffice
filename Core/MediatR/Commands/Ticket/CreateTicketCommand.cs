using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using MediatR;

namespace BoxOffice.Core.MediatR.Commands.Ticket
{
    public class CreateTicketCommand : IRequest<TicketDto>
    {
        public int SpectacleId { get; set; }
        public int Seat { get; set; }

        public Client Client;
    }
}
