using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.FavoriteJourney
{
    public record FavoriteJourneyCommand(Guid JourneyId, Guid UserId) : ICommand<bool>;

}
