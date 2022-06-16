using BoxOffice.Core.Dto;
using BoxOffice.Core.MediatR.Queries.Spectacle;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Spectacle
{
    public class GetAllSpectaclesHandler : IRequestHandler<GetAllSpectaclesQuery, List<SpectacleDto>>
    {
        private readonly ISpectacleService _service;

        public GetAllSpectaclesHandler(ISpectacleService service)
        {
            _service = service;
        }

        public Task<List<SpectacleDto>> Handle(GetAllSpectaclesQuery request, CancellationToken cancellationToken)
        {
            return _service.GetAll();
        }
    }
}
