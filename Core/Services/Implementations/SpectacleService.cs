using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Repositories.Interfaces;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISpectacleRepository _spectacleRepository;

        public SpectacleService(AppDbContext context, IMapper mapper, ISpectacleRepository spectacleRepository)
        {
            _context = context;
            _mapper = mapper;
            _spectacleRepository = spectacleRepository;
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

            //var result = _context.Spectacles.Add(data);
            //await _context.SaveChangesAsync();
            //return _mapper.Map<SpectacleDto>(result.Entity);

            return _mapper.Map<SpectacleDto>(await _spectacleRepository.Create(data));
        }

        public async Task<List<SpectacleDto>> GetAll()
        {
            var result = await _spectacleRepository.GetAll();
            return _mapper.Map<List<SpectacleDto>>(result.ToList());
        }

        public async Task<SpectacleDto> GetById(int id)
        {
            var result = await _spectacleRepository.GetById(id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            return _mapper.Map<SpectacleDto>(result);
        }

        public async Task<string> RemoveAsync(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            await _spectacleRepository.Delete(id);
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
            return _mapper.Map<SpectacleDto>(data);
        }
    }
}

