using MediatR;
using System;

namespace BoxOffice.Core.MediatR.Queries.Spectacle
{
    public interface ICacheableMediatrQuery<T> : IRequest<T>
    {
        bool BypassCache { get; }
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
