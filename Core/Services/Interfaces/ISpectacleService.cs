using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ISpectacleService
    {
        Task<SpectacleDto> CreateAsync(CreateSpectacle model, Admin admin);
        Task<Stream> CreateCsvFileAsync();
        Task<Stream> CreateXmlFileAsync();
        Task<List<SpectacleDto>> CreateFromCsv(IFormFile file, Admin admin);
        Task<List<SpectacleDto>> CreateFromXml(IFormFile file, Admin admin);
        Task<List<SpectacleDto>> GetAll();
        Task<SpectacleDto> GetById(int id);
        Task<string> RemoveAsync(int id);
        Task<SpectacleDto> UpdateAsync(SpectacleDto model);
    }
}