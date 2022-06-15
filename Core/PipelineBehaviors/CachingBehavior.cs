using BoxOffice.Core.Queries;
using BoxOffice.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.PipelineBehaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : ICacheableMediatrQuery<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
        private readonly CacheSettings _settings;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger, IConfiguration config)
        {
            _logger = logger;
            _cache = cache;
            _settings = config.GetRequiredSection("CacheSettings").Get<CacheSettings>();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {            
            if (request.BypassCache) 
                return await next();
            TResponse response = default(TResponse);
            var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache(request, cancellationToken, next);
                _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }
            return response;
        }

        private async Task<TResponse> GetResponseAndAddToCache(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = await next();
            var slidingExpiration = request.SlidingExpiration == null ? TimeSpan.FromHours(_settings.SlidingExpiration) : request.SlidingExpiration;
            var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
            await _cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
            return response;
        }
    }
}
