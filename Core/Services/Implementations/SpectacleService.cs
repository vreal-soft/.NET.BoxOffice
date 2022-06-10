using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class SpectacleService : ISpectacleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SpectacleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return _mapper.Map<SpectacleDto>(result.Entity);
        }

        public Task<List<SpectacleDto>> GetAll()
        {
            //using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            //        .AddSqlClientInstrumentation(opt => opt.Enrich = (activity, eventName, rawObject) =>
            //        {
            //            if (eventName.Equals("OnCustom"))
            //            {
            //                if (rawObject is SqlCommand cmd)
            //                {
            //                    activity.SetTag("db.commandTimeout", cmd.CommandTimeout);
            //                }
            //            };
            //        })
            //        .Build();
            var activitySource = new ActivitySource("SampleActivitySource");


            using (var sampleActivity = activitySource.StartActivity("Sample", ActivityKind.Server))
            {
                // note that "sampleActivity" can be null here if nobody listen events generated
                // by the "SampleActivitySource" activity source.
                sampleActivity?.AddTag("Name", "Task<List<SpectacleDto>> GetAll");
                sampleActivity?.AddBaggage("SampleContext", "SpectacleService : ISpectacleService");              
            }
            var result = _context.Spectacles.ToList();

            return Task.FromResult(_mapper.Map<List<SpectacleDto>>(result));
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

