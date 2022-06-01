using AutoMapper;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Services.Provaiders;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly TokenProvider _tokenProvider;

        public AuthService(AppDbContext context, IMapper mapper, TokenProvider tokenProvider)
        {
            _context = context;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
        }

        public Task<ClientDto> ClientRegistration(Registration model)
        {
            model.Email = model.Email.ToLower();
            var client = _context.Clients.FirstOrDefault(x => x.Email == model.Email);
            if (client != null)
                throw new AppException("Client already registered.");

            var newClient = _mapper.Map<Client>(model);
            newClient.Hash = PasswordManager.HashPassword(model.Password);
            var result = _context.Clients.Add(newClient);
            _context.SaveChanges();

            return Task.FromResult(_mapper.Map<ClientDto>(result));
        }

        public async Task<Token> ClientLogin(Login model)
        {
            var client = _context.Clients.FirstOrDefault(x => x.Email == model.Email);
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
    }
}
