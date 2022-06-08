using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ISpectacleService
    {
        Task<Dto.SpectacleDto> CreateAsync(CreateSpectacle model, Admin admin);
        Task<List<Dto.SpectacleDto>> GetAll();
        Task<Data.Entities.SpectacleDto> GetById(int id);
        Task<string> RemoveAsync(int id);
        Task<Data.Entities.SpectacleDto> UpdateAsync(Dto.SpectacleDto model);
    }
}