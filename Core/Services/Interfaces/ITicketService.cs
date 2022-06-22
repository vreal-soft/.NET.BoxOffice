using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ITicketService
    {
        Task<List<TicketDto>> GetAllInSpectacle(string spectacleId);
        Task<FreePlace> GetFreePlaces(string spectacleId);
        Task<List<TicketDto>> GetAllInClient(Client client);
        Task<TicketDto> GetById(string id);
        Task<List<TicketDto>> GetAll();
        Task BuyAsync(BuyTicket model, Client client);
        Task<string> Refund(string ticketId, Client client);
    }
}