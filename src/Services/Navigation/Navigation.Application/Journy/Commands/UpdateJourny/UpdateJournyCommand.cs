using BuildingBlocks.CQRS;
using FluentValidation;
using Navigation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.UpdateJourny
{

    public record UpdateJournyCommand(JourneyDto JourneyDto) : ICommand<UpdateJournyResult>;
    public record UpdateJournyResult(bool IsSuccess);
    public class UpdateJournyCommandValidator : AbstractValidator<UpdateJournyCommand>
    {
        public UpdateJournyCommandValidator()
        {
            RuleFor(x => x.JourneyDto.StartLocation).NotEmpty().WithMessage("StartLocation is required");
            RuleFor(x => x.JourneyDto.EndLocation).NotNull().WithMessage("EndLocation is required");
            RuleFor(x => x.JourneyDto.DistanceInKilometers).NotEmpty().WithMessage("DistanceInKilometers is required");
            RuleFor(x => x.JourneyDto.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
