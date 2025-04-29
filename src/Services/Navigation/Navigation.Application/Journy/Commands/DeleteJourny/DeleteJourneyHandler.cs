using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.DeleteJourny
{
    public class DeleteJourneyHandler(IApplicationDbContext dbContext)
     : ICommandHandler<DeleteJourneyCommand, DeleteJourneyResult>
    {
        public async Task<DeleteJourneyResult> Handle(DeleteJourneyCommand command, CancellationToken cancellationToken)
        {
            var journeyId = JourneyId.Of(command.JourneyId);
            var journey = await dbContext.Journeys
                .FindAsync([journeyId], cancellationToken: cancellationToken);

            if (journey is null)
            {
                throw new Exception("journey not found to delete  with id :" +command.JourneyId);
            }

            dbContext.Journeys.Remove(journey);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteJourneyResult(true);
        }
    }
}
