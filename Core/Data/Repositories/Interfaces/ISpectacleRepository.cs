using BoxOffice.Core.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Data.Repositories.Interfaces
{
    public interface ISpectacleRepository
    {
        Task<Spectacle> Create(Spectacle model);
        Task<IEnumerable<Spectacle>> GetAll();
        Task<Spectacle> GetById(int spectacleId);
        Task<int> Update(Spectacle model);
        Task Delete(int spectacleId);
    }
}
