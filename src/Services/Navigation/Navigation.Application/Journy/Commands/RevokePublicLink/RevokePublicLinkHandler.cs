using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.RevokePublicLink
{
    public class RevokeJourneyPublicLinkHandler(IApplicationDbContext dbContext) : ICommandHandler<RevokeJourneyPublicLinkCommand, bool>
    {
        public async Task<bool> Handle(RevokeJourneyPublicLinkCommand command, CancellationToken cancellationToken)
        {
            var journey = await dbContext.Journeys.FindAsync([JourneyId.Of(command.JourneyId)], cancellationToken);
            if (journey == null) throw new Exception("Journey not found");

            journey.RevokePublicShareLink(command.Link);
            dbContext.Journeys.Update(journey);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
