using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Commands.Spectacle;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ISpectacleService
    {
        Task<SpectacleDto> CreateAsync(CreateSpectacleCommand model, Admin admin);
        Task<List<SpectacleDto>> GetAll();
        Task<SpectacleDto> GetById(int id);
        Task<string> RemoveAsync(int id);
        Task<SpectacleDto> UpdateAsync(UpdateSpectacleCommand model);
    }
}