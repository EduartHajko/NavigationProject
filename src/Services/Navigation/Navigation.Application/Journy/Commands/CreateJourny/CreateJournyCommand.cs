using BuildingBlocks.CQRS;
using FluentValidation;
using Navigation.Application.Dtos;

namespace Navigation.Application.Journy.Commands.CreateJourny
{
     public record CreateJournyCommand(JourneyDto JourneyDto): ICommand<CreateJournyResult>;
     public record CreateJournyResult(Guid Id);
    public class CreateJournyCommandValidator : AbstractValidator<CreateJournyCommand>
    {
        public CreateJournyCommandValidator()
        {
            RuleFor(x => x.JourneyDto.StartLocation).NotEmpty().WithMessage("StartLocation is required");
            RuleFor(x => x.JourneyDto.EndLocation).NotNull().WithMessage("EndLocation is required");
            RuleFor(x => x.JourneyDto.DistanceInKilometers).NotEmpty().WithMessage("DistanceInKilometers is required");
            RuleFor(x => x.JourneyDto.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
