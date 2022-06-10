using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly string KEY;
        public SpectacleService(AppDbContext context, IMapper mapper, IDistributedCache cache)
        {
            KEY = $"{GetType().Name}.GetAll";
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<SpectacleDto> CreateAsync(CreateSpectacle model, Admin admin)
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
            await _cache.RemoveAsync(KEY);
            return _mapper.Map<SpectacleDto>(result.Entity);
        }

        public async Task<List<SpectacleDto>> GetAll()
        {
            var cach = await _cache.GetAsync(KEY);

            if (cach != null)            
                return JsonConvert.DeserializeObject<List<SpectacleDto>>(Encoding.UTF8.GetString(cach));            

            var list = _context.Spectacles.ToList();
            var serialized = JsonConvert.SerializeObject(list);
            var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(1))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(12));
            await _cache.SetAsync(KEY, Encoding.UTF8.GetBytes(serialized), options);
            return _mapper.Map<List<SpectacleDto>>(list);
        }

        public Task<SpectacleDto> GetById(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            return Task.FromResult(_mapper.Map<SpectacleDto>(result));
        }

        public async Task<string> RemoveAsync(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            _context.Spectacles.Remove(result);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(KEY);
            return "The model has been removed.";
        }

        public async Task<SpectacleDto> UpdateAsync(SpectacleDto model)
        {
            var data = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == model.Id);

            if (data == null)
                throw new AppException($"Model with id {model.Id} does not exist.");
            else if (model.TotalTicket < data.Tickets.Count)
                throw new AppException($"You have already sold tickets for {data.Tickets.Count} seats.");

            data = _mapper.Map(model, data);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(KEY);
            return _mapper.Map<SpectacleDto>(data);
        }
    }
}

