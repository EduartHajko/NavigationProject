using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Administration.Api.Endpoints.Handlers
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

            // In a real implementation, we would:
            // 1. Update user statistics in the database
            // 2. Award a badge to the user
            // 3. Send a notification to the user
            // 4. Update leaderboards or other gamification features

            return Task.CompletedTask;
        }
    }
}
