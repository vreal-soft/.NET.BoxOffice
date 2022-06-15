using MediatR;
using System;

namespace BoxOffice.Core.Queries
{
    public interface ICacheableMediatrQuery<T> : IRequest<T>
    {
        bool BypassCache { get; }
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
