using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BoxOffice.Core.Data.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connString;
        public AdminRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public async Task<Admin> Create(Admin model)
        {
            using IDbConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "INSERT INTO public.\"Admins\"(\"FirstName\", \"LastName\", \"Email\", \"Hash\") " +
                            "VALUES(@FirstName, @LastName, @Email, @Hash) RETURNING \"Id\"";
            model.Id = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, model);
            return model;
        }

        public async Task Delete(int adminId)
        {
            using NpgsqlConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "DELETE FROM public.\"Admins\" WHERE \"Id\" = @Id; ";
            await db.QueryAsync(sqlQuery, new { Id = adminId });
        }

        public Task<IList<Admin>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Admin> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Update(Admin model)
        {
            throw new System.NotImplementedException();
        }
    }
}
