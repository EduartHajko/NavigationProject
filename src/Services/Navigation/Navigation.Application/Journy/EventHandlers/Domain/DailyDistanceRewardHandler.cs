using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Navigation.Application.Data;
using Navigation.Domain.Events;

namespace Navigation.Application.Journy.EventHandlers.Domain
{
    /// <summary>
    /// Handles journey events to check if a user has exceeded the daily distance goal of 20 km
    /// and awards a badge when the threshold is crossed.
    /// </summary>
    public class DailyDistanceRewardHandler : 
        INotificationHandler<JourneyCreatedEvent>,
        INotificationHandler<JourneyUpdatedEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DailyDistanceRewardHandler> _logger;
        private const double DAILY_GOAL_THRESHOLD_KM = 20.0;

        public DailyDistanceRewardHandler(
            IApplicationDbContext dbContext,
            ILogger<DailyDistanceRewardHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task Handle(JourneyCreatedEvent notification, CancellationToken cancellationToken)
        {
            return ProcessJourneyForDailyGoal(notification.Journey.UserId, notification.Journey.StartTime.Date, notification.Journey.Id.Value, cancellationToken);
        }

        public Task Handle(JourneyUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return ProcessJourneyForDailyGoal(notification.Journey.UserId, notification.Journey.StartTime.Date, notification.Journey.Id.Value, cancellationToken);
        }

        /// <summary>
        /// Processes a journey to check if the user has exceeded the daily distance goal
        /// </summary>
        private async Task ProcessJourneyForDailyGoal(Guid userId, DateTime journeyDate, Guid journeyId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Checking daily goal for user {UserId} on {Date}", userId, journeyDate.ToShortDateString());

                // Check if the user already has a journey that triggered the daily goal for this date
                var alreadyAchieved = await _dbContext.Journeys
                    .AnyAsync(j => j.UserId == userId && 
                                  j.StartTime.Date == journeyDate.Date && 
                                  j.DailyGoalTriggered, 
                           cancellationToken);

                if (alreadyAchieved)
                {
                    _logger.LogInformation("User {UserId} has already achieved daily goal on {Date}", userId, journeyDate.ToShortDateString());
                    return;
                }

                // Calculate total distance for the user on this date
                var totalDistance = await _dbContext.Journeys
                    .Where(j => j.UserId == userId && j.StartTime.Date == journeyDate.Date)
                    .SumAsync(j => j.DistanceInKilometers, cancellationToken);

                _logger.LogInformation("User {UserId} has traveled {Distance} km on {Date}", userId, totalDistance, journeyDate.ToShortDateString());

                // Check if the total distance exceeds the threshold
                if (totalDistance >= DAILY_GOAL_THRESHOLD_KM)
                {
                    // Find the journey that triggered the goal (the one that pushed the total over the threshold)
                    var journeyToMark = await _dbContext.Journeys
                        .FirstOrDefaultAsync(j => j.Id.Value == journeyId, cancellationToken);

                    if (journeyToMark != null)
                    {
                        _logger.LogInformation("Marking journey {JourneyId} as daily goal trigger for user {UserId} on {Date}", 
                            journeyId, userId, journeyDate.ToShortDateString());
                        
                        // Mark the journey as the daily goal trigger
                        journeyToMark.MarkAsDailyGoalTrigger();
                        
                        // Save changes
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        
                        _logger.LogInformation("Daily goal achieved for user {UserId} on {Date}", userId, journeyDate.ToShortDateString());
                    }
                }
                else
                {
                    _logger.LogInformation("User {UserId} has not yet reached the daily goal of {Threshold} km on {Date}", 
                        userId, DAILY_GOAL_THRESHOLD_KM, journeyDate.ToShortDateString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing daily goal for user {UserId} on {Date}", userId, journeyDate.ToShortDateString());
                throw;
            }
        }
    }
}
