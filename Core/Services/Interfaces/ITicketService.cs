using BoxOffice.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IList<TicketDto>> GetAllInSpectacle(int spectacleId);
        Task<FreePlace> GetFreePlaces(int spectacleId);
        Task<IList<TicketDto>> GetAllInClient();
        Task<TicketDto> GetById(int id);
        Task<IList<TicketDto>> GetAll();
        Task<TicketDto> Buy(BuyTicket model);
        Task<string> Refund(int ticketId);
    }
}