

namespace BuildingBlocks.Messaging.Events
{
    /// <summary>
    /// Integration event that is published when a user achieves their daily distance goal
    /// </summary>
    public record DailyGoalAchievedEvent(Guid UserId, Guid JourneyId, DateTime Date, double DistanceInKilometers) : IntegrationEvent;

}
