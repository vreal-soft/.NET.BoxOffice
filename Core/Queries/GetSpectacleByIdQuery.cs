using BoxOffice.Core.Dto;
using MediatR;

namespace BoxOffice.Core.Queries
{
    public class GetSpectacleByIdQuery : IRequest<SpectacleDto>
    {
        public int Id { get; }

        public GetSpectacleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
