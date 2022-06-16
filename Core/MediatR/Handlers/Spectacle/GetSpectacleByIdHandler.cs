using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Queries.Spectacle;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Spectacle
{
    public class GetSpectacleByIdHandler : IRequestHandler<GetSpectacleByIdQuery, SpectacleDto>
    {
        private readonly ISpectacleService _service;

        public GetSpectacleByIdHandler(ISpectacleService service)
        {
            _service = service;
        }

        public Task<SpectacleDto> Handle(GetSpectacleByIdQuery request, CancellationToken cancellationToken)
        {
            return _service.GetById(request.Id);
        }
    }
}
