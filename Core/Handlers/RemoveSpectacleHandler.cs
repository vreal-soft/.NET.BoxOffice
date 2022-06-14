using BoxOffice.Core.Commands;
using BoxOffice.Core.Dto;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.Handlers
{
    public class RemoveSpectacleHandler : IRequestHandler<RemoveSpectacleCommand, string>
    {
        private readonly ISpectacleService _service;

        public RemoveSpectacleHandler(ISpectacleService service)
        {
            _service = service;

        }

        public Task<string> Handle(RemoveSpectacleCommand request, CancellationToken cancellationToken)
        {
            return _service.RemoveAsync(request.Id);
        }
    }
}
