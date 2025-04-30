using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.UnfavoriteJourney
{
    public class UnfavoriteJourneyHandler(IApplicationDbContext dbContext) : ICommandHandler<UnfavoriteJourneyCommand, bool>
    {
        public async Task<bool> Handle(UnfavoriteJourneyCommand command, CancellationToken cancellationToken)
        {
            var journey = await dbContext.Journeys.FindAsync([JourneyId.Of(command.JourneyId)], cancellationToken);
            if (journey == null) throw new Exception("Journey not found");

            journey.UnmarkFavorite(command.UserId);
            dbContext.Journeys.Update(journey);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
