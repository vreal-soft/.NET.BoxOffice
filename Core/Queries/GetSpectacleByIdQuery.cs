using BoxOffice.Core.Dto;
using MediatR;
using System;

namespace BoxOffice.Core.Queries
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
