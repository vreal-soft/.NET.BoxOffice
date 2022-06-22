using BoxOffice.Core.Dto;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Token> ClientLogin(Login model);
        Task ClientRegistrationAsync(Registration model);
        Task AdminRegistrationAsync(Registration model);
        Task<Token> AdminLogin(Login model);
    }
}