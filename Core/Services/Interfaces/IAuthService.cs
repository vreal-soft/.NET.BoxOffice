﻿using BoxOffice.Core.Dto;
using System.Threading.Tasks;

namespace BoxOffice.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Token> ClientLogin(Login model);
        Task<ClientDto> ClientRegistration(Registration model);
        Task<AdminDto> AdminRegistration(Registration model);
        Task<Token> AdminLogin(Login model);
    }
}