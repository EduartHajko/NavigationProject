using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Application.Dtos;
using Navigation.Domain.Enums;
using Navigation.Domain.Model;
using Navigation.Domain.ValueObjects;


namespace Navigation.Application.Journy.Commands.UpdateJourny
{
    public class UpdateJourneyHandler(IApplicationDbContext dbContext)
     : ICommandHandler<UpdateJournyCommand, UpdateJournyResult>
    {
        public async Task<UpdateJournyResult> Handle(UpdateJournyCommand command, CancellationToken cancellationToken)
        {
            var journeyId = JourneyId.Of(command.JourneyDto.Id);
            var journey = await dbContext.Journeys.FindAsync([journeyId], cancellationToken: cancellationToken);


            if (journey == null)
            {
                throw new Exception("JOURNY NOT FOUND  ID: " + journeyId );
            }
            UpdateJourneyWithNewValues(journey, command.JourneyDto);

            dbContext.Journeys.Update(journey);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateJournyResult(true);
        }

        private void UpdateJourneyWithNewValues(Journey journey, JourneyDto dto)
        {
            var start = new Location(dto.StartLocation.Latitude, dto.StartLocation.Longitude, dto.StartLocation.Name);
            var end = new Location(dto.EndLocation.Latitude, dto.EndLocation.Longitude, dto.EndLocation.Name);
            var transport = Enum.Parse<TransportType>(dto.TransportType, ignoreCase: true);

            journey.Update(
                startLocation: start,
                endLocation: end,
                startTime: dto.StartTime,
                endTime: dto.EndTime,
                transportType: transport,
                distanceInKilometers: dto.DistanceInKilometers
            );
        }
    }
}
