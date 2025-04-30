using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.FavoriteJourney
{
    public class FavoriteJourneyHandler(IApplicationDbContext dbContext) : ICommandHandler<FavoriteJourneyCommand, bool>
    {
        public async Task<bool> Handle(FavoriteJourneyCommand command, CancellationToken cancellationToken)
        {
            var journey = await dbContext.Journeys.FindAsync([JourneyId.Of(command.JourneyId)], cancellationToken);
            if (journey == null) throw new Exception("Journey not found");

            journey.MarkAsFavorite(command.UserId);
            dbContext.Journeys.Update(journey);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
