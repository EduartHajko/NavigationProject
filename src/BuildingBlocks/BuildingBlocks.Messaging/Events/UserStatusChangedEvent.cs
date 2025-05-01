using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record UserStatusChangedEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public UserStatus OldStatus { get; init; }
        public UserStatus NewStatus { get; init; }
        public DateTime ChangedAt { get; init; }

        public UserStatusChangedEvent(Guid userId, UserStatus oldStatus, UserStatus newStatus)
        {
            UserId = userId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            ChangedAt = DateTime.UtcNow;
        }
    }
}
