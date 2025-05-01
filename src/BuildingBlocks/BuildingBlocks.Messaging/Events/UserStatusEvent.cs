

namespace BuildingBlocks.Messaging.Events
{
    public enum UserStatus
    {
        Active,
        Suspended,
        Deactivated
    }

    public record UserStatusEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public UserStatus Status { get; init; }

        public UserStatusEvent(Guid userId, UserStatus status)
        {
            UserId = userId;
            Status = status;
        }
    }
}
