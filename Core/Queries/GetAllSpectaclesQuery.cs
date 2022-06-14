using BoxOffice.Core.Dto;
using MediatR;
using System.Collections.Generic;

namespace BoxOffice.Core.Queries
{
    public class GetAllSpectaclesQuery : IRequest<List<SpectacleDto>>
    {
    }
}
