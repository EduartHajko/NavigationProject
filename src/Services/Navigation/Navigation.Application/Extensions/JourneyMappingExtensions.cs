using Navigation.Application.Dtos;
using Navigation.Domain.Model;

namespace Navigation.Application.Extensions
{
    public static class JourneyMappingExtensions
    {
        public static List<JourneyDto> ToJourneyDtoList(this IEnumerable<Journey> journeys)
            => journeys.Select(j => j.ToDto()).ToList();

        public static JourneyDto ToDto(this Journey j) =>
            new(
                j.Id.Value,
                j.UserId,
                new LocationDto(j.StartLocation.Latitude, j.StartLocation.Longitude, j.StartLocation.Name),
                new LocationDto(j.EndLocation.Latitude, j.EndLocation.Longitude, j.EndLocation.Name),
                j.StartTime,
                j.EndTime,
                j.TransportType.ToString(),
                j.DistanceInKilometers,
                j.DailyGoalTriggered,
                j.CreatedAt,
                j.LastModified
            );
    }

}
