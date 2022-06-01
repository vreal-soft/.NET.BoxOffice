using BoxOffice.Core.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface ISpectacleService
    {
        Task<SpectacleDto> Create(CreateSpectacle model);
        Task<IList<SpectacleDto>> GetAll();
        Task<SpectacleDto> GetById(int id);
        Task<string> Remove(int id);
        Task<SpectacleDto> Update(SpectacleDto model);
    }
}