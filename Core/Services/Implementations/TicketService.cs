using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public TicketService(AppDbContext context, IMapper mapper, IHttpContextAccessor accessor)
        {
            _context = context;
            _mapper = mapper;
            _accessor = accessor;
        }

        public Task<FreePlace> GetFreePlaces(int spectacleId)
        {
            var spectacle = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == spectacleId);
            if (spectacle == null)
                throw new AppException($"Model with id {spectacleId} does not exist.");

            if (spectacle.TotalTicket == spectacle.Tickets.Count)
                return Task.FromResult(new FreePlace());

            var soldTickets = spectacle.Tickets.Select(x => x.Seat).ToArray();
            FreePlace freePlace = new FreePlace();
            freePlace.Seats = new List<int>();

            for (int i = 1; i <= spectacle.TotalTicket; i++)
            {
                if (!soldTickets.Contains(i))
                    freePlace.Seats.Add(i);
            }

            return Task.FromResult(freePlace);
        }

        public Task<IList<TicketDto>> GetAllInClient()
        {
            var client = GetCurrentClient();
            var tickets = _context.Tickets.Include(x => x.Spectacle).Include(x => x.Client)
                            .Where(x => x.ClientId == client.Id).ToList();
            return Task.FromResult(_mapper.Map<IList<TicketDto>>(tickets));
        }

        public Task<IList<TicketDto>> GetAllInSpectacle(int spectacleId)
        {
            var tickets = _context.Tickets.Include(x => x.Spectacle).Include(x => x.Client)
                            .Where(x => x.SpectacleId == spectacleId).ToList();
            return Task.FromResult(_mapper.Map<IList<TicketDto>>(tickets));
        }

        public Task<IList<TicketDto>> GetAll()
        {
            return Task.FromResult(_mapper.Map<IList<TicketDto>>(
                _context.Tickets.Include(x => x.Spectacle).Include(x => x.Client).ToList())
                );
        }

        public Task<TicketDto> GetById(int id)
        {
            var ticket = _context.Tickets.Include(x => x.Spectacle).Include(x => x.Client).FirstOrDefault(x => x.Id == id);
            return Task.FromResult(_mapper.Map<TicketDto>(ticket));
        }


        public Task<TicketDto> Buy(BuyTicket model)
        {
            var spectacle = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == model.SpectacleId);
            var client = GetCurrentClient();

            if (spectacle == null)
                throw new AppException($"Model with id {model.SpectacleId} does not exist.");
            else if (spectacle.Tickets.Any(x => x.Seat == model.Seat))
                throw new AppException($"Sorry, {model.Seat} place is already taken.");

            var newTicket = new Ticket()
            {
                Client = client,
                Seat = model.Seat,
                Spectacle = spectacle
            };
            var result = _context.Tickets.Add(newTicket);
            _context.SaveChanges();

            return Task.FromResult(_mapper.Map<TicketDto>(result.Entity));
        }

        public Task<string> Refund(int ticketId)
        {
            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == ticketId);
            var client = GetCurrentClient();
            if (ticket.ClientId != client.Id)
                throw new AppException($"Model with id {ticketId} does not exist.");

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return Task.FromResult("The ticket has been successfully returned to the ticket office.");
        }

        private Client GetCurrentClient()
        {
            if (_accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role) != "Client")
                throw new AppException("Invalid role.");

            if (!int.TryParse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
                throw new AppException("Invalid token data.");

            var client = _context.Clients.FirstOrDefault(x => x.Id == id);

            if (client == null)
                throw new AppException("Invalid token data.");

            return client;
        }
    }
}
