using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Repositories.Interfaces;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BoxOffice.Core.Data.Repositories.Implementations
{
    public class TicketRepositoty : ITicketRepositoty
    {
        private readonly string _connString;
        public TicketRepositoty(string connectionString)
        {
            _connString = connectionString;
        }

        public async Task<Ticket> Create(Ticket model)
        {
            using NpgsqlConnection db = new(_connString);
            var sqlQuery = "INSERT INTO public.\"Tickets\"(\"ClientId\", \"SpectacleId\", \"Seat\") VALUES(@ClientId, @SpectacleId, @Seat) RETURNING \"Id\";";
            model.Id = await db.QueryFirstOrDefaultAsync<int>(sqlQuery, model);
            return model;
        }

        public async Task Delete(int ticketId)
        {
            using NpgsqlConnection db = new(_connString);
            var sqlQuery = "DELETE FROM public.\"Tickets\" WHERE \"Id\" = @Id; ";
            await db.QueryAsync(sqlQuery, new { Id = ticketId });
        }

        public IEnumerable<Ticket> GetAll()
        {
            using NpgsqlConnection db = new(_connString);
            db.Open();
            var sqlQuery =
                "SELECT \"Tickets\".\"Id\", \"ClientId\", \"SpectacleId\", \"Seat\", \"Spectacles\".\"Name\", \"Spectacles\".\"StartTime\", " +
                "\"Spectacles\".\"EndTime\", \"Clients\".\"FirstName\", \"Clients\".\"LastName\"" +
                "FROM public.\"Tickets\"" +
                "INNER JOIN public.\"Spectacles\" ON public.\"Tickets\".\"SpectacleId\" = public.\"Spectacles\".\"Id\"" +
                "INNER JOIN public.\"Clients\" ON public.\"Tickets\".\"ClientId\" = public.\"Clients\".\"Id\"";

            var t = db.Query<Ticket, Spectacle, Client, Ticket>(sqlQuery, (ticket, spectacle, client) =>
            {
                ticket.Spectacle = spectacle;
                ticket.Client = client;
                return ticket;
            },
            splitOn: "SpectacleId,ClientId");
            db.Close();
            return db.Query<Ticket, Spectacle, Client, Ticket>(sqlQuery, (ticket, spectacle, client) =>
            {
                ticket.Spectacle = spectacle;
                ticket.Client = client;
                return ticket;
            });
        }

        public async Task<Ticket> GetById(int ticketId)
        {
            using NpgsqlConnection db = new(_connString);
            var sqlQuery =
                "SELECT \"Tickets\".\"Id\", \"ClientId\", \"SpectacleId\", \"Seat\", \"Spectacles\".\"Name\", \"Spectacles\".\"StartTime\", " +
                "\"Spectacles\".\"EndTime\", \"Clients\".\"FirstName\", \"Clients\".\"LastName\"" +
                "FROM public.\"Tickets\"" +
                "LEFT JOIN public.\"Spectacles\" ON public.\"Tickets\".\"SpectacleId\" = public.\"Spectacles\".\"Id\"" +
                "LEFT JOIN public.\"Clients\" ON public.\"Tickets\".\"ClientId\" = public.\"Clients\".\"Id\"" +
                "WHERE public.\"Tickets\".\"Id\" = @Id";

            var list = await db.QueryAsync<Ticket, Spectacle, Client, Ticket>(sqlQuery, (ticket, spectacle, client) =>
            {
                ticket.Spectacle = spectacle;
                ticket.Client = client;
                return ticket;
            },
            param: new { Id = ticketId },
            splitOn: "SpectacleId,ClientId");

            return list.FirstOrDefault();
        }
    }
}
