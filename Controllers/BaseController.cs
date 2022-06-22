using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Data.Settings;
using BoxOffice.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;
using System.Security.Claims;

namespace BoxOffice.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMongoCollection<Admin> _admins;
        private readonly IMongoCollection<Client> _clients;

        public BaseController(IHttpContextAccessor accessor, SpectacleDatabaseSettings settings)
        {
            _accessor = accessor;
            var client = new MongoClient(settings.ConnectionURI);
            var database = client.GetDatabase(settings.DatabaseName);
            _admins = database.GetCollection<Admin>("admins");
            _clients = database.GetCollection<Client>("clients");
        }

        protected Admin GetCurrentAdmin()
        {
            if (_accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role) != "Admin")
                throw new AppException("Invalid role.");
            var id = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id))
                throw new AppException("Invalid token data.");

            var admin = _admins.Find(x => x.Id == id).FirstOrDefault();

            if (admin == null)
                throw new AppException("Invalid token data.");

            return admin;
        }

        protected Client GetCurrentClient()
        {
            if (_accessor.HttpContext.User.FindFirstValue(ClaimTypes.Role) != "Client")
                throw new AppException("Invalid role.");
            var id = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id))
                throw new AppException("Invalid token data.");

            var client = _clients.Find(x => x.Id == id).FirstOrDefault();

            if (client == null)
                throw new AppException("Invalid token data.");

            return client;
        }
    }
}
