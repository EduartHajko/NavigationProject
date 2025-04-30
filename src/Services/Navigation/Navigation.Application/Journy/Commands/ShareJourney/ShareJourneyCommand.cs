using BuildingBlocks.CQRS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.ShareJourney
{
    public record ShareJourneyCommand(Guid JourneyId, List<Guid> UserIds) : ICommand<bool>;
    public class ShareJourneyCommandValidator : AbstractValidator<ShareJourneyCommand>
    {
        public ShareJourneyCommandValidator()
        {
            RuleFor(x => x.JourneyId).NotEmpty();
            RuleFor(x => x.UserIds).NotEmpty();
        }
    }
}
