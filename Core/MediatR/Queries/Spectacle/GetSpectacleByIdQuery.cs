using BoxOffice.Core.Dto;
using System;

namespace BoxOffice.Core.MediatR.Queries.Spectacle
{
    public class GetSpectacleByIdQuery : ICacheableMediatrQuery<SpectacleDto>
    {
        public int Id { get; set; }
        public bool BypassCache { get; set; }
        public string CacheKey => $"Spectacle-{Id}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetSpectacleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
