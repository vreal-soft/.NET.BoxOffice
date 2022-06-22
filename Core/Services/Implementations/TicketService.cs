using AutoMapper;
using AutoMapper.QueryableExtensions;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Settings;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BoxOffice.Core.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Ticket> _tickets;
        private readonly IMongoCollection<Spectacle> _spectacle;

        public TicketService(IMapper mapper, SpectacleDatabaseSettings settings)
        {
            _mapper = mapper;

            var client = new MongoClient(settings.ConnectionURI);
            var database = client.GetDatabase(settings.DatabaseName);
            _tickets = database.GetCollection<Ticket>("tickets");
            _spectacle = database.GetCollection<Spectacle>("spectacles");
        }

        public Task<FreePlace> GetFreePlaces(string spectacleId)
        {
            var spectacle = _spectacle.Find(x => x.Id == spectacleId).FirstOrDefault();
            if (spectacle == null)
                throw new AppException($"Model with id {spectacleId} does not exist.");

            if (spectacle.TotalTicket == spectacle.Tickets.Count)
                return Task.FromResult(new FreePlace());

            var soldTickets = spectacle.Tickets.Select(x => x.Seat).ToArray();
            return Task.FromResult(FindAvailablePlaces(spectacle, soldTickets));
        }

        public Task<List<TicketDto>> GetAllInClient(Client client)
        {
            //var tickets = _tickets.Find(x => x.ClientId == client.Id).AsQueryable().ProjectTo<TicketDto>(_mapper.ConfigurationProvider).ToList();
            IMongoQueryable<Ticket> tt = _tickets.AsQueryable();
            var tickets = tt.Where(x => x.ClientId == client.Id).ProjectTo<TicketDto>(_mapper.ConfigurationProvider).ToList();
            return Task.FromResult(tickets);
        }

        public Task<List<TicketDto>> GetAllInSpectacle(string spectacleId)
        {
            //var tickets = _context.Tickets.Where(x => x.SpectacleId == spectacleId).ProjectTo<TicketDto>(_mapper.ConfigurationProvider).ToList();
            IMongoQueryable<Ticket> tt = _tickets.AsQueryable();
            var tickets = tt.Where(x => x.SpectacleId == spectacleId).ProjectTo<TicketDto>(_mapper.ConfigurationProvider).ToList();
            return Task.FromResult(tickets);
        }

        public Task<List<TicketDto>> GetAll()
        {
            IMongoQueryable<Ticket> tt = _tickets.AsQueryable();
            return Task.FromResult(tt.ProjectTo<TicketDto>(_mapper.ConfigurationProvider).ToList());
        }

        public Task<TicketDto> GetById(string id)
        {
            IMongoQueryable<Ticket> tt = _tickets.AsQueryable();
            var ticket = tt.ProjectTo<TicketDto>(_mapper.ConfigurationProvider).FirstOrDefault(x => x.Id == id);
            if (ticket == null)
                throw new AppException($"Model with id {id} does not exist.");
            return Task.FromResult(_mapper.Map<TicketDto>(ticket));
        }


        public async Task BuyAsync(BuyTicket model, Client client)
        {
            var spectacle = _spectacle.Find(x => x.Id == model.SpectacleId).FirstOrDefault();

            if (spectacle == null)
                throw new AppException($"Model with id {model.SpectacleId} does not exist.");
            else if (spectacle.Tickets.Any(x => x.Seat == model.Seat))
                throw new AppException($"Sorry, {model.Seat} place is already taken.");

            var newTicket = new Ticket()
            {
                ClientId = client.Id,
                Client = client,
                Seat = model.Seat,
                SpectacleId = spectacle.Id,
                Spectacle = spectacle
            };

            await _tickets.InsertOneAsync(newTicket);
        }

        public async Task<string> Refund(string ticketId, Client client)
        {
            var ticket = _tickets.Find(x => x.Id == ticketId).FirstOrDefault();
            if (ticket.ClientId != client.Id)
                throw new AppException($"Model with id {ticketId} does not exist.");

            var result = await _tickets.DeleteOneAsync(x => x.Id == ticketId);
            if (result.DeletedCount < 0)
                throw new AppException($"Model with id {ticketId} does not exist.");

            return "The ticket has been successfully returned to the ticket office.";
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
