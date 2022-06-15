using BoxOffice.Core.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace BoxOffice.Core.Queries
{
    public class GetAllSpectaclesQuery : ICacheableMediatrQuery<List<SpectacleDto>>
    {
        public int Id { get; set; }
        public bool BypassCache { get; set; }
        public string CacheKey => $"SpectacleList";
        public TimeSpan? SlidingExpiration { get; set; }
    }
}
