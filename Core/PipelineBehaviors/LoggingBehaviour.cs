using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.PipelineBehaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //Request
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            IList<PropertyInfo> props = new List<PropertyInfo>(request.GetType().GetProperties());
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(request, null);
                sb.AppendLine($"{prop.Name} : {propValue}");               
            }
            _logger.LogInformation(sb.ToString());
            var response = await next();
            //Response
            _logger.LogInformation($"Handled {typeof(TResponse).Name}");
            return response;
        }
    }
}
