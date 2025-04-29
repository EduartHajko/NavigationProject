using BuildingBlocks.CQRS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.DeleteJourny
{
    public record DeleteJourneyCommand(Guid JourneyId)
     : ICommand<DeleteJourneyResult>;

    public record DeleteJourneyResult(bool IsSuccess);
    public class DeleteJourneyCommandValidator : AbstractValidator<DeleteJourneyCommand>
    {
        public DeleteJourneyCommandValidator()
        {
            RuleFor(x => x.JourneyId)
                .NotEmpty()
                .WithMessage("JourneyId is required");
        }
    }
}
