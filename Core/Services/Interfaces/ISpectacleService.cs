using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ISpectacleService
    {
        Task CreateAsync(CreateSpectacle model, Admin admin);
        Task<List<SpectacleDto>> GetAll();
        Task<SpectacleDto> GetById(string id);
        Task<string> RemoveAsync(string id);
        Task<SpectacleDto> UpdateAsync(SpectacleDto model);
    }
}