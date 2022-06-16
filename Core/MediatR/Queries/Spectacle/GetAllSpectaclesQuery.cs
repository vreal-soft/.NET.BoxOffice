using BoxOffice.Core.Dto;
using System;
using System.Collections.Generic;

namespace BoxOffice.Core.MediatR.Queries.Spectacle
{
    public class GetAllSpectaclesQuery : ICacheableMediatrQuery<List<SpectacleDto>>
    {
        public bool BypassCache { get; set; }
        public string CacheKey => $"SpectacleList";
        public TimeSpan? SlidingExpiration { get; set; }
    }
}
