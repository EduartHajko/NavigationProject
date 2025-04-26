using Navigation.Domain.Abstractions;
using Navigation.Domain.Model;
using Navigation.Domain.ValueObjects;


namespace Navigation.Domain.Events
{
    public record JourneyUpdatedEvent(Journey Journey) : IDomainEvent;


}
