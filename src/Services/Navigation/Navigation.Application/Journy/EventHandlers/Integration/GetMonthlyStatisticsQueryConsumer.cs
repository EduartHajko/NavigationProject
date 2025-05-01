using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Navigation.Application.Data;

namespace Navigation.Application.Journy.EventHandlers.Integration
{
    public record MonthlyDistanceDto(
        Guid UserId,
        int Year,
        int Month,
        double TotalDistanceKm
    );

    public record GetMonthlyDistanceResult(
        List<MonthlyDistanceDto> Statistics,
        int TotalCount
    );

    public class GetMonthlyStatisticsQueryConsumer : IConsumer<GetMonthlyStatisticsEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<GetMonthlyStatisticsQueryConsumer> _logger;

        public GetMonthlyStatisticsQueryConsumer(
            IApplicationDbContext dbContext,
            ILogger<GetMonthlyStatisticsQueryConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GetMonthlyStatisticsEvent> context)
        {
            _logger.LogInformation("Consuming GetMonthlyStatisticsEvent");
            
            var query = context.Message;

            // Group journeys by user, year, and month to calculate total distance
            var monthlyStatsQuery = _dbContext.Journeys
                .GroupBy(j => new { 
                    j.UserId, 
                    Year = j.StartTime.Year, 
                    Month = j.StartTime.Month 
                })
                .Select(g => new MonthlyDistanceDto(
                    g.Key.UserId,
                    g.Key.Year,
                    g.Key.Month,
                    g.Sum(j => j.DistanceInKilometers)
                ))
                .AsQueryable();

            // Apply sorting
            monthlyStatsQuery = query.OrderBy?.ToLower() switch
            {
                "userid" => monthlyStatsQuery.OrderBy(s => s.UserId),
                "totaldistancekm" => monthlyStatsQuery.OrderBy(s => s.TotalDistanceKm),
                "totaldistancekm_desc" => monthlyStatsQuery.OrderByDescending(s => s.TotalDistanceKm),
                _ => monthlyStatsQuery.OrderByDescending(s => s.TotalDistanceKm) // Default sorting
            };

            // Get total count before pagination
            var totalCount = await monthlyStatsQuery.CountAsync(context.CancellationToken);

            // Apply pagination
            var paged = await monthlyStatsQuery
                .Skip(query.Page * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(context.CancellationToken);

            await context.RespondAsync(new GetMonthlyDistanceResult(paged, totalCount));
        }
    }
}
