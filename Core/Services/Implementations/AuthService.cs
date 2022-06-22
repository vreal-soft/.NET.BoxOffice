using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Services.Provaiders;
using BoxOffice.Core.Shared;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BoxOffice.Core.Data.Settings;
using MongoDB.Driver;

namespace BoxOffice.Core.Services.Implementations
{
    public class AuthService : IAuthService
    {      
        private readonly IMapper _mapper;
        private readonly ITokenProvider _tokenProvider;
        private readonly IMongoCollection<Admin> _admins;
        private readonly IMongoCollection<Client> _clients;

        public AuthService(IMapper mapper, ITokenProvider tokenProvider, SpectacleDatabaseSettings settings)
        {            
            _mapper = mapper;
            _tokenProvider = tokenProvider;

            var client = new MongoClient(settings.ConnectionURI);
            var database = client.GetDatabase(settings.DatabaseName);
            _admins = database.GetCollection<Admin>("admins");
            _clients = database.GetCollection<Client>("clients");
        }

        public async Task ClientRegistrationAsync(Registration model)
        {
            model.Email = model.Email.ToLower();
            var client = _clients.Find(x => x.Email == model.Email).FirstOrDefault();
            if (client != null)
                throw new AppException("Client already registered.");

            var newClient = _mapper.Map<Client>(model);
            newClient.Hash = PasswordManager.HashPassword(model.Password);

            await _clients.InsertOneAsync(newClient);
        }

        public async Task<Token> ClientLogin(Login model)
        {
            var client = _clients.Find(x => x.Email == model.Email).FirstOrDefault();
            if (client == null || !PasswordManager.VerifyPassword(model.Password, client.Hash))
                throw new AppException("Invalid login or password.");
            var claims = new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                            new Claim(ClaimTypes.Email, client.Email),
                            new Claim(ClaimTypes.Role, "Client"),
                        };
            return await _tokenProvider.CreateTokensAsync(claims);
        }

        public async Task AdminRegistrationAsync(Registration model)
        {
            model.Email = model.Email.ToLower();
            var admin = _admins.Find(x => x.Email == model.Email).FirstOrDefault();
            if (admin != null)
                throw new AppException("Admin already registered.");

            var newAdmin = _mapper.Map<Admin>(model);
            newAdmin.Hash = PasswordManager.HashPassword(model.Password);
            await _admins.InsertOneAsync(newAdmin);
        }

        public async Task<Token> AdminLogin(Login model)
        {
            var admin = _admins.Find(x => x.Email == model.Email).FirstOrDefault();
            if (admin == null || !PasswordManager.VerifyPassword(model.Password, admin.Hash))
                throw new AppException("Invalid login or password.");
            var claims = new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                            new Claim(ClaimTypes.Email, admin.Email),
                            new Claim(ClaimTypes.Role, "Admin"),
                        };
            return await _tokenProvider.CreateTokensAsync(claims);
        }
    }
}
