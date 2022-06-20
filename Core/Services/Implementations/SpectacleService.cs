using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

        public async Task<Stream> CreateCsvFileAsync()
        {
            var list = _mapper.Map<List<CreateSpectacle>>(_context.Spectacles);

            using var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(list);
                writer.Flush();
            }

            return new MemoryStream(stream.ToArray());
        }

        public Task<Stream> CreateXmlFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SpectacleDto>> CreateFromXml(IFormFile file, Admin admin)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SpectacleDto>> CreateFromCsv(IFormFile file, Admin admin)
        {
            if (file == null || Path.GetExtension(file.FileName).Substring(1) != "csv")
                throw new AppException("File Extension Is InValid - Only Upload CSV File");
            List<CreateSpectacle> models;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                models = csv.GetRecords<CreateSpectacle>().ToList();
            }

            return await RunPgSqlTransaction(models, admin, _context.Database.GetConnectionString());
        }

        private async Task<List<SpectacleDto>> RunPgSqlTransaction(List<CreateSpectacle> models, Admin admin, string myConnString)
        {
            List<SpectacleDto> data = new();
            NpgsqlCommand pgCommand = new()
            {
                Connection = new NpgsqlConnection(myConnString)
            };
            pgCommand.Connection.Open();
            pgCommand.Transaction = pgCommand.Connection.BeginTransaction();

            try
            {
                foreach (var item in models)
                    data.Add(await CreateAsync(item, admin));
            }
            catch (Exception ex)
            {
                pgCommand.Transaction.Rollback();
                throw new AppException(ex.Message);
            }
            finally
            {
                pgCommand.Connection.Close();
            }

            return data;
        }

        public Task<List<SpectacleDto>> GetAll()
        {
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

