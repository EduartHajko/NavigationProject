using Navigation.Domain.Abstractions;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.Events
{
    public record JourneyFavoritedEvent(JourneyId JourneyId, Guid UserId, DateTime Timestamp) : IDomainEvent;

}
