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
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public SpectacleService(AppDbContext context, IMapper mapper, IHttpContextAccessor accessor)
        {
            _context = context;
            _mapper = mapper;
            _accessor = accessor;
        }

        public Task<SpectacleDto> Create(CreateSpectacle model)
        {
            var data = _mapper.Map<Spectacle>(model);
            data.AdminId = GetCurrentAdminId();
            if (data.StartTime > data.EndTime)
                throw new AppException("The time is incorrect.");
            var result = _context.Spectacles.Add(data);
            _context.SaveChanges();
            return Task.FromResult(_mapper.Map<SpectacleDto>(result));
        }

        public Task<IList<SpectacleDto>> GetAll()
        {
            var result = _context.Spectacles.ToList();
            return Task.FromResult(_mapper.Map<IList<SpectacleDto>>(result));
        }

        public Task<SpectacleDto> GetById(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            return Task.FromResult(_mapper.Map<SpectacleDto>(result));
        }

        public Task<string> Remove(int id)
        {
            var result = _context.Spectacles.FirstOrDefault(x => x.Id == id);
            if (result == null)
                throw new AppException($"Model with id {id} does not exist.");
            _context.Spectacles.Remove(result);
            _context.SaveChanges();
            return Task.FromResult("The model has been removed.");
        }

        public Task<SpectacleDto> Update(SpectacleDto model)
        {
            var data = _context.Spectacles.Include(x => x.Tickets).FirstOrDefault(x => x.Id == model.Id);
            if (data == null)
                throw new AppException($"Model with id {model.Id} does not exist.");

            data = _mapper.Map(model, data);

            if (data.StartTime > data.EndTime)
                throw new AppException("The time is incorrect.");
            else if (data.TotalTicket < data.Tickets.Count)
                throw new AppException("The problem is with the number of tickets.");

            var result = _context.Spectacles.Update(data);
            _context.SaveChanges();
            return Task.FromResult(_mapper.Map<SpectacleDto>(result));
        }

        private int GetCurrentAdminId()
        {
            if (_accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role) != "Client")
            {
                if (!int.TryParse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
                    throw new AppException("Invalid token data.");

                return id;
            }
            throw new AppException("Invalid role.");
        }
    }
}
