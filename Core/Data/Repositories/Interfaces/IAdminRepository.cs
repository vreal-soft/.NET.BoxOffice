using BoxOffice.Core.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Data.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> Create(Admin model);
        Task<IList<Admin>> GetAll();
        Task<Admin> GetById(int id);
        Task<int> Update(Admin model);
        Task Delete(int adminId);
    }
}
