using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Administration.Api.Consumers
{
    /// <summary>
    /// Consumes DailyGoalAchievedEvent integration events to update user statistics and badges
    /// </summary>
    public class DailyGoalAchievedEventConsumer : IConsumer<DailyGoalAchievedEvent>
    {
        private readonly ILogger<DailyGoalAchievedEventConsumer> _logger;

        public DailyGoalAchievedEventConsumer(ILogger<DailyGoalAchievedEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<DailyGoalAchievedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation(
                "Daily goal achieved by user {UserId} on {Date} with journey {JourneyId} and distance {Distance} km",
                @event.UserId, @event.Date.ToShortDateString(), @event.JourneyId, @event.DistanceInKilometers);

            return Task.CompletedTask;
        }
    }
}
