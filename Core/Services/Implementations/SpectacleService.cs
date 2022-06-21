using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Settings;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Spectacle> _spectacle;

        public SpectacleService(AppDbContext context, IMapper mapper, SpectacleDatabaseSettings settings)
        {
            _context = context;
            _mapper = mapper;

            var client = new MongoClient(settings.ConnectionURI);
            var database = client.GetDatabase(settings.DatabaseName);
            _spectacle = database.GetCollection<Spectacle>(settings.CollectionName);
        }

        public async Task CreateAsync(CreateSpectacle model, Admin admin)
        {
            var data = _mapper.Map<Spectacle>(model);
            data.AdminId = admin.Id;
            bool IsTimeBusy = _spectacle.Find(emp => true).ToList().Any(x =>
                (x.StartTime <= data.StartTime && x.EndTime >= data.StartTime) ||
                (x.StartTime <= data.EndTime && x.EndTime >= data.EndTime) ||
                (data.StartTime <= x.StartTime && data.EndTime >= x.EndTime));

            if (IsTimeBusy)
                throw new AppException("This time is busy.");

            await _spectacle.InsertOneAsync(data);          
        }

        public Task<List<SpectacleDto>> GetAll()
        {
            //var result = _context.Spectacles.ToList();
            return Task.FromResult(_mapper.Map<List<SpectacleDto>>(_spectacle.Find(emp => true).ToList()));
        }

        public Task<SpectacleDto> GetById(string id)
        {
            //var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            var result = _spectacle.Find(x => x.Id == id).FirstOrDefault();
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            return Task.FromResult(_mapper.Map<SpectacleDto>(result));
        }

        public async Task<string> RemoveAsync(string id)
        {
            var result = await _spectacle.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount < 0)
                throw new AppException($"Model with id {id} does not exist.");           
            return "The model has been removed.";
        }

        public async Task<SpectacleDto> UpdateAsync(SpectacleDto model)
        {
            var data = _spectacle.Find(x => x.Id == model.Id).FirstOrDefault();

            if (data == null)
                throw new AppException($"Model with id {model.Id} does not exist.");
            else if (model.TotalTicket < data.Tickets.Count)
                throw new AppException($"You have already sold tickets for {data.Tickets.Count} seats.");

            data = _mapper.Map(model, data);
            await _spectacle.ReplaceOneAsync(x => x.Id == model.Id, data);
            return _mapper.Map<SpectacleDto>(data);
        }
    }
}

