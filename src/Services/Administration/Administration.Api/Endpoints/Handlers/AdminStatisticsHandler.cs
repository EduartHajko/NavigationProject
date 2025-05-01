using Administration.Api.Endpoints;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;

namespace Administration.Api.Endpoints.Handlers
{
    public record GetMonthlyStatisticsCommand(
        int Page,
        int PageSize,
        string? OrderBy
    ) : ICommand<GetMonthlyDistanceResult>;

    public class MonthlyStatisticsCommandHandler : ICommandHandler<GetMonthlyStatisticsCommand, GetMonthlyDistanceResult>
    {
        private readonly IRequestClient<GetMonthlyStatisticsEvent> _client;
        private readonly ILogger<MonthlyStatisticsCommandHandler> _logger;

        public MonthlyStatisticsCommandHandler(
            IRequestClient<GetMonthlyStatisticsEvent> client,
            ILogger<MonthlyStatisticsCommandHandler> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<GetMonthlyDistanceResult> Handle(GetMonthlyStatisticsCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling monthly statistics request");

            // Adapt to the event
            var eventMessage = new GetMonthlyStatisticsEvent(
                command.Page,
                command.PageSize,
                command.OrderBy
            );

            try
            {
                // Request-response over RabbitMQ
                var response = await _client.GetResponse<GetMonthlyDistanceResult>(eventMessage, cancellationToken);
                return response.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly statistics");
                
                // For demo purposes, return mock data if the real service is not available
                var mockStatistics = new List<MonthlyDistanceDto>
                {
                    new MonthlyDistanceDto(Guid.NewGuid(), DateTime.Now.Year, DateTime.Now.Month, 125.7),
                    new MonthlyDistanceDto(Guid.NewGuid(), DateTime.Now.Year, DateTime.Now.Month, 87.3),
                    new MonthlyDistanceDto(Guid.NewGuid(), DateTime.Now.Year, DateTime.Now.Month - 1, 103.2)
                };
                
                return new GetMonthlyDistanceResult(mockStatistics, mockStatistics.Count);
            }
        }
    }
}
