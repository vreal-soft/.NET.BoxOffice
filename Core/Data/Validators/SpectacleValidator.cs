using BoxOffice.Core.MediatR.Commands.Spectacle;
using FluentValidation;

namespace BoxOffice.Core.Data.Validators
{
    public class CreateSpectacleValidator : AbstractValidator<CreateSpectacleCommand>
    {
        public CreateSpectacleValidator()
        {
            RuleFor(x => x.StartTime)
                .NotNull().GreaterThan((ulong)0).WithMessage("Please input start time.");

            RuleFor(x => x.EndTime)
                .NotNull().GreaterThan((ulong)0).WithMessage("Please input start time.");

            RuleFor(x => x.TotalTicket)
                .NotNull().GreaterThan((uint)0).WithMessage("Enter the exact number of tickets.");

            RuleFor(x => x).Must(x => x.StartTime < x.EndTime)
                .WithMessage("The time is incorrect.");
        }
    }
    public class UpdateSpectacleValidator : AbstractValidator<UpdateSpectacleCommand>
    {
        public UpdateSpectacleValidator()
        {
            RuleFor(x => x.StartTime)
                .NotNull().GreaterThan((ulong)0).WithMessage("Please input start time.");

            RuleFor(x => x.EndTime)
                .NotNull().GreaterThan((ulong)0).WithMessage("Please input start time.");

            RuleFor(x => x.TotalTicket)
                .NotNull().GreaterThan((uint)0).WithMessage("Enter the exact number of tickets.");

            RuleFor(x => x).Must(x => x.StartTime < x.EndTime)
                .WithMessage("The time is incorrect.");
        }
    }
}
