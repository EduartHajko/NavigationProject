using Navigation.Domain.Abstractions;
using Navigation.Domain.Enums;
using Navigation.Domain.ValueObjects;

namespace Navigation.Domain.Events
{

    public record UserStatusChangedEvent(
     UserId UserId,
     UserStatus NewStatus,
     DateTime ChangedAt
 ) : IDomainEvent;
}
