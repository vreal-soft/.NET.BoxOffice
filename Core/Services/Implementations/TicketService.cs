using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TicketService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<FreePlace> GetFreePlaces(int spectacleId)
        {
            var spectacle = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == spectacleId);
            if (spectacle == null)
                throw new AppException($"Model with id {spectacleId} does not exist.");

            if (spectacle.TotalTicket == spectacle.Tickets.Count)
                return Task.FromResult(new FreePlace());

            var soldTickets = spectacle.Tickets.Select(x => x.Seat).ToArray();
            return Task.FromResult(FindAvailablePlaces(spectacle, soldTickets));
        }

        public Task<List<TicketDto>> GetAllInClient(Client client)
        {
            var tickets = _context.Tickets.Where(x => x.ClientId == client.Id).ToList();
            return Task.FromResult(_mapper.Map<List<TicketDto>>(tickets));
        }

        public Task<List<TicketDto>> GetAllInSpectacle(int spectacleId)
        {
            var tickets = _context.Tickets.Include(x => x.Spectacle).Where(x => x.SpectacleId == spectacleId).ToList();
            return Task.FromResult(_mapper.Map<List<TicketDto>>(tickets));
        }

        public Task<List<TicketDto>> GetAll()
        {
            var tickets = _context.Tickets.Include(x => x.Client).Include(x => x.Spectacle).ToList();
            return Task.FromResult(_mapper.Map<List<TicketDto>>(tickets));
        }

        public Task<TicketDto> GetById(int id)
        {
            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == id);
            if (ticket == null)
                throw new AppException($"Model with id {id} does not exist.");
            return Task.FromResult(ticket.Adapt<TicketDto>());
        }


        public async Task<TicketDto> BuyAsync(BuyTicket model, Client client)
        {
            var spectacle = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == model.SpectacleId);

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
            await _context.SaveChangesAsync();

            return result.Entity.Adapt<TicketDto>();
        }

        public Task<string> Refund(int ticketId, Client client)
        {
            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == ticketId);
            if (ticket.ClientId != client.Id)
                throw new AppException($"Model with id {ticketId} does not exist.");

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return Task.FromResult("The ticket has been successfully returned to the ticket office.");
        }

        private static FreePlace FindAvailablePlaces(Spectacle spectacle, int[] soldTickets)
        {
            FreePlace freePlace = new();

            for (int i = 1; i <= spectacle.TotalTicket; i++)
            {
                if (!soldTickets.Contains(i))
                    freePlace.Seats.Add(i);
            }

            return freePlace;
        }
    }
}
