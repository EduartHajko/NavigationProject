using BuildingBlocks.CQRS;
using Navigation.Application.Data;
using Navigation.Application.Dtos;
using Navigation.Domain.Enums;
using Navigation.Domain.Model;
using Navigation.Domain.ValueObjects;

namespace Navigation.Application.Journy.Commands.CreateJourny
{
    public class CreateJourneyHandler(IApplicationDbContext dbContext)
     : ICommandHandler<CreateJournyCommand, CreateJournyResult>
    {
        public async Task<CreateJournyResult> Handle(CreateJournyCommand command, CancellationToken cancellationToken)
        {
            var journey = CreateNewJourney(command.JourneyDto);

            dbContext.Journeys.Add(journey);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateJournyResult(journey.Id.Value);
        }

        private Journey CreateNewJourney(JourneyDto dto)
        {
            var start = new Location(dto.StartLocation.Latitude, dto.StartLocation.Longitude, dto.StartLocation.Name);
            var end = new Location(dto.EndLocation.Latitude, dto.EndLocation.Longitude, dto.EndLocation.Name);

            var transportType = Enum.Parse<TransportType>(dto.TransportType, ignoreCase: true);

            return Journey.Create(
                id: JourneyId.Of(Guid.NewGuid()),
                userId: dto.UserId,
                startLocation: start,
                endLocation: end,
                startTime: dto.StartTime,
                endTime: dto.EndTime,
                transportType: transportType,
                distanceInKilometers: dto.DistanceInKilometers
            );
        }
    }
}
