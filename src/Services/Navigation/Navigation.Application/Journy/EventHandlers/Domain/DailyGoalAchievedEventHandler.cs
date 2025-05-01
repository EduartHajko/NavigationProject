using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Navigation.Domain.Events;

namespace Navigation.Application.Journy.EventHandlers.Domain
{
    /// <summary>
    /// Handles the DailyGoalAchievedEvent and publishes an integration event for other services
    /// </summary>
    public class DailyGoalAchievedEventHandler : INotificationHandler<DailyGoalAchievedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<DailyGoalAchievedEventHandler> _logger;

        public DailyGoalAchievedEventHandler(
            IPublishEndpoint publishEndpoint,
            ILogger<DailyGoalAchievedEventHandler> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Handle(DailyGoalAchievedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event handled: {DomainEvent} for user {UserId} on {Date}",
                notification.GetType().Name, notification.UserId, notification.Date.ToShortDateString());

            // Create an integration event to notify other services
                 var integrationEvent = new DailyGoalAchievedEvent(
                 notification.UserId,
                 notification.JourneyId,
                 notification.Date,
                 notification.DistanceInKilometers
                 );


            // Publish the integration event
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);

            _logger.LogInformation("Published integration event for daily goal achievement: User {UserId} on {Date}",
                notification.UserId, notification.Date.ToShortDateString());
        }
    }
}
