using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Commands.Spectacle;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Spectacle
{
    public class UpdateSpectacleHandler : IRequestHandler<UpdateSpectacleCommand, SpectacleDto>
    {
        private readonly ISpectacleService _service;

        public UpdateSpectacleHandler(ISpectacleService service, IHttpContextAccessor accessor)
        {
            _service = service;
        }

        public Task<SpectacleDto> Handle(UpdateSpectacleCommand request, CancellationToken cancellationToken)
        {
            return _service.UpdateAsync(request);
        }
    }
}
