using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Commands.Spectacle;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public SpectacleService(AppDbContext context, IMapper mapper, IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<SpectacleDto> CreateAsync(CreateSpectacleCommand model, Admin admin)
        {
            var data = _mapper.Map<Spectacle>(model);
            data.AdminId = admin.Id;
            bool IsTimeBusy = _context.Spectacles.Any(x =>
                (x.StartTime <= data.StartTime && x.EndTime >= data.StartTime) ||
                (x.StartTime <= data.EndTime && x.EndTime >= data.EndTime) ||
                (data.StartTime <= x.StartTime && data.EndTime >= x.EndTime));

            if (IsTimeBusy)
                throw new AppException("This time is busy.");

            var result = _context.Spectacles.Add(data);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("SpectacleList");
            return _mapper.Map<SpectacleDto>(result.Entity);
        }

        public Task<List<SpectacleDto>> GetAll()
        {
            var result = _context.Spectacles.ToList();
            return Task.FromResult(_mapper.Map<List<SpectacleDto>>(result));
        }

        public Task<SpectacleDto> GetById(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(_mapper.Map<SpectacleDto>(result));
        }

        public async Task<string> RemoveAsync(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            _context.Spectacles.Remove(result);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("SpectacleList");
            await _cache.RemoveAsync($"Spectacle-{id}");
            return "The model has been removed.";
        }

        public async Task<SpectacleDto> UpdateAsync(UpdateSpectacleCommand model)
        {
            var data = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == model.Id);

            if (data == null)
                throw new AppException($"Model with id {model.Id} does not exist.");
            else if (model.TotalTicket < data.Tickets.Count)
                throw new AppException($"You have already sold tickets for {data.Tickets.Count} seats.");

            data = _mapper.Map(model, data);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("SpectacleList");
            await _cache.RemoveAsync($"Spectacle-{model.Id}");
            return _mapper.Map<SpectacleDto>(data);
        }
    }
}

