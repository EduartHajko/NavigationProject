using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.ShareJourney
{
    public class ShareJourneyHandler(IApplicationDbContext dbContext)
    : ICommandHandler<ShareJourneyCommand, bool>
    {
        public async Task<bool> Handle(ShareJourneyCommand command, CancellationToken cancellationToken)
        {
            var journeyId = JourneyId.Of(command.JourneyId);
            var journey = await dbContext.Journeys.FindAsync([journeyId], cancellationToken);

            if (journey == null)
                throw new Exception("Journey not found");

            foreach (var userId in command.UserIds)
            {
                journey.ShareWithUser(userId); 
            }

            dbContext.Journeys.Update(journey);
            await dbContext.SaveChangesAsync(cancellationToken); // Triggers domain events

            return true;
        }
    }

}
