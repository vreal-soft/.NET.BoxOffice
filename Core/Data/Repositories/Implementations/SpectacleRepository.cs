using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Repositories.Interfaces;
using Dapper;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxOffice.Core.Data.Repositories.Implementations
{
    public class SpectacleRepository : ISpectacleRepository
    {
        private readonly string _connString;
        public SpectacleRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public async Task<Spectacle> Create(Spectacle model)
        {
            using NpgsqlConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "INSERT INTO public.\"Spectacles\"(\"Name\", \"Description\", \"TotalTicket\", \"StartTime\", \"EndTime\", \"AdminId\") " +
                            "VALUES(@Name, @Description, @TotalTicket, @StartTime, @EndTime, @AdminId) RETURNING \"Id\"";


            //NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, db);
            //db.Open();
            //cmd.Parameters.AddWithValue("Name", model.Name);
            //cmd.Parameters.AddWithValue("Description", model.Description);
            //cmd.Parameters.AddWithValue("TotalTicket", model.TotalTicket);
            //cmd.Parameters.AddWithValue("AdminId", model.AdminId);

            //cmd.Parameters.AddWithValue("StartTime", model.StartTime);
            //cmd.Parameters.AddWithValue("EndTime", model.EndTime);
            //cmd.Parameters.Add(new NpgsqlParameter("StartTime", NpgsqlDbType.Numeric));
            //cmd.Parameters.Add(new NpgsqlParameter("EndTime", NpgsqlDbType.Numeric));
            //cmd.Parameters.Add(new NpgsqlParameter("TotalTicket", NpgsqlDbType.Bigint));
            //cmd.Parameters.Add(new NpgsqlParameter("AdminId", NpgsqlDbType.Integer));
            //var t = await cmd.ExecuteReaderAsync();
            //db.Close();


            model.Id = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, model);
            return model;
        }

        public async Task Delete(int spectacleId)
        {
            using NpgsqlConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "DELETE FROM public.\"Spectacles\" WHERE \"Id\" = @Id; ";
            await db.QueryAsync(sqlQuery, new { Id = spectacleId });
        }

        public async Task<IEnumerable<Spectacle>> GetAll()
        {
            using NpgsqlConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "SELECT * FROM public.\"Spectacles\" ORDER BY \"Id\" ASC";
            return await db.QueryAsync<Spectacle>(sqlQuery);
        }

        public async Task<Spectacle> GetById(int spectacleId)
        {
            using NpgsqlConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "SELECT * FROM public.\"Spectacles\" WHERE \"Id\" = @Id;";
            return await db.QueryFirstOrDefaultAsync<Spectacle>(sqlQuery, new { Id = spectacleId });
        }

        public async Task<int> Update(Spectacle model)
        {
            using NpgsqlConnection db = new NpgsqlConnection(_connString);
            var sqlQuery = "UPDATE public.\"Spectacles\" SET \"Name\" = @Name, \"Description\" = @Description, \"TotalTicket\" = @TotalTicket, " +
                                                                                            "\"StartTime\" = @StartTime, \"EndTime\" = @EndTime, \"AdminId\" = @AdminId" +
                                                                                            "WHERE \"Id\" = @Id;";
            return await db.ExecuteAsync(sqlQuery, model);
        }
    }
}
