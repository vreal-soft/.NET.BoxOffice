using BoxOffice.Core.Commands;
using BoxOffice.Core.Data;
using BoxOffice.Core.Data.Entities;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using BoxOffice.Core.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.Handlers
{
    public class CreateSpectacleHandler : IRequestHandler<CreateSpectacleCommand, SpectacleDto>
    {
        private readonly ISpectacleService _service;     

        public CreateSpectacleHandler(ISpectacleService service, IHttpContextAccessor accessor)
        {
            _service = service;         
            
        }

        public Task<SpectacleDto> Handle(CreateSpectacleCommand request, CancellationToken cancellationToken)
        {
            return _service.CreateAsync(request, request.admin);
        }
    }
}
