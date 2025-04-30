using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.UnfavoriteJourney
{
    public record UnfavoriteJourneyCommand(Guid JourneyId, Guid UserId) : ICommand<bool>;

}
