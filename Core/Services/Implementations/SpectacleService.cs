using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;        
        public SpectacleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dto.SpectacleDto> CreateAsync(CreateSpectacle model, Admin admin)
        {
            var data = model.Adapt<Spectacle>();
            data.AdminId = admin.Id;
            bool IsTimeBusy = _context.Spectacles.Any(x =>
                (x.StartTime <= data.StartTime && x.EndTime >= data.StartTime) ||
                (x.StartTime <= data.EndTime && x.EndTime >= data.EndTime) ||
                (data.StartTime <= x.StartTime && data.EndTime >= x.EndTime));

            if (IsTimeBusy)
                throw new AppException("This time is busy.");

            var result = _context.Spectacles.Add(data);
            await _context.SaveChangesAsync();
            return result.Entity.Adapt<Dto.SpectacleDto>();
        }

        public Task<List<Dto.SpectacleDto>> GetAll()
        {
            var result = _context.Spectacles.ToList();
            return Task.FromResult(result.Adapt<List<Dto.SpectacleDto>>());
        }

        public Task<Data.Entities.SpectacleDto> GetById(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            return Task.FromResult(result.AdaptToDto());
        }

        public async Task<string> RemoveAsync(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            _context.Spectacles.Remove(result);
            await _context.SaveChangesAsync();
            return "The model has been removed.";
        }

        public async Task<Data.Entities.SpectacleDto> UpdateAsync(Dto.SpectacleDto model)
        {
            var data = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == model.Id);

            if (data == null)
                throw new AppException($"Model with id {model.Id} does not exist.");
            else if (model.TotalTicket < data.Tickets.Count)
                throw new AppException($"You have already sold tickets for {data.Tickets.Count} seats.");

            data = model.Adapt<Spectacle>();
            await _context.SaveChangesAsync();
            return data.AdaptToDto();
        }
    }
}

