using BoxOffice.Core.MediatR.Commands.Spectacle;
using BoxOffice.Core.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BoxOffice.Core.MediatR.Handlers.Spectacle
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
