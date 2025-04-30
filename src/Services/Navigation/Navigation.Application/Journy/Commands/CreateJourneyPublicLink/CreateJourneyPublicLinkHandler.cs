using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.CreateJourneyPublicLink
{
    public class CreateJourneyPublicLinkHandler(IApplicationDbContext dbContext)
     : ICommandHandler<CreateJourneyPublicLinkCommand, string>
    {
        public async Task<string> Handle(CreateJourneyPublicLinkCommand command, CancellationToken cancellationToken)
        {
            var journeyId = JourneyId.Of(command.JourneyId);
            var journey = await dbContext.Journeys.FindAsync([journeyId], cancellationToken);

            if (journey == null)
                throw new Exception("Journey not found");

            var link = journey.CreatePublicShareLink(); 

            dbContext.Journeys.Update(journey);
            await dbContext.SaveChangesAsync(cancellationToken); // Dispatches event

            return link;
        }
    }
}
