using MediatR;

namespace BoxOffice.Core.MediatR.Commands.Spectacle
{
    public class RemoveSpectacleCommand : IRequest<string>
    {
        public int Id { get; set; }

        public RemoveSpectacleCommand(int id)
        {
            Id = id;
        }
    }
}
