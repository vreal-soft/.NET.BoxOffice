using BoxOffice.Core.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Data.Repositories.Interfaces
{
    public interface ITicketRepositoty
    {
        Task<Ticket> Create(Ticket model);
        IEnumerable<Ticket> GetAll();
        Task<Ticket> GetById(int ticketId);       
        Task Delete(int ticketId);
    }
}
