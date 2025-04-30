using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Navigation.Application.Data;
using Navigation.Application.Dtos;

namespace Navigation.Application.Journy.EventHandlers.Integration
{
    public record GetAdminJourneysResult(
        List<JourneyAdminDto> Journeys,
        int TotalCount
    );
    public class GetAdminJourneysQueryConsumer(IApplicationDbContext dbContext) : IConsumer<GetAdminJourneysEvent>
    {
        public async Task Consume(ConsumeContext<GetAdminJourneysEvent> context)
        {
            var query = context.Message;

            var journeys = dbContext.Journeys.AsQueryable();

            // Apply filters
            if (query.UserId.HasValue)
                journeys = journeys.Where(j => j.UserId == query.UserId);

            if (!string.IsNullOrWhiteSpace(query.TransportType))
                journeys = journeys.Where(j => j.TransportType.ToString() == query.TransportType);

            if (query.StartDateFrom.HasValue)
                journeys = journeys.Where(j => j.StartTime >= query.StartDateFrom.Value);

            if (query.StartDateTo.HasValue)
                journeys = journeys.Where(j => j.StartTime <= query.StartDateTo.Value);

            if (query.ArrivalDateFrom.HasValue)
                journeys = journeys.Where(j => j.EndTime >= query.ArrivalDateFrom.Value);

            if (query.ArrivalDateTo.HasValue)
                journeys = journeys.Where(j => j.EndTime <= query.ArrivalDateTo.Value);

            if (query.MinDistance.HasValue)
                journeys = journeys.Where(j => j.DistanceInKilometers >= query.MinDistance.Value);

            if (query.MaxDistance.HasValue)
                journeys = journeys.Where(j => j.DistanceInKilometers <= query.MaxDistance.Value);

            // Apply sorting
            journeys = (query.OrderBy?.ToLower(), query.Direction?.ToLower()) switch
            {
                ("userid", "desc") => journeys.OrderByDescending(j => j.UserId),
                ("userid", _) => journeys.OrderBy(j => j.UserId),

                ("totaldistancekm", "desc") => journeys.OrderByDescending(j => j.DistanceInKilometers),
                ("totaldistancekm", _) => journeys.OrderBy(j => j.DistanceInKilometers),

                ("starttime", "desc") => journeys.OrderByDescending(j => j.StartTime),
                ("starttime", _) => journeys.OrderBy(j => j.StartTime),

                _ => journeys.OrderBy(j => j.StartTime)
            };

            // Get total count before pagination
            var totalCount = await journeys.CountAsync();

            // Apply pagination
            var paged = await journeys
                .Skip(query.Page * query.PageSize)
                .Take(query.PageSize)
                .Select(j => new JourneyAdminDto(
                    j.Id.Value,
                    j.UserId,
                    j.TransportType.ToString(),
                    j.StartTime,
                    j.EndTime,
                    j.DistanceInKilometers
                ))
                .ToListAsync();

            await context.RespondAsync(new GetAdminJourneysResult(paged, totalCount));
        }
    }


}
