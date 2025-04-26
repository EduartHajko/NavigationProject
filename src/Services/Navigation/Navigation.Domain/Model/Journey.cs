using Navigation.Domain.Abstractions;
using Navigation.Domain.Enums;
using Navigation.Domain.Events;
using Navigation.Domain.Model.Navigation.Domain.Models;
using Navigation.Domain.ValueObjects;

namespace Navigation.Domain.Model
{
    public class Journey : Aggregate<JourneyId>
    {
        private readonly List<JourneyShare> _sharedWithUsers = new();
        private readonly List<JourneyPublicLink> _publicShareLinks = new();

        public IReadOnlyCollection<JourneyShare> SharedWithUsers => _sharedWithUsers.AsReadOnly();
        public IReadOnlyCollection<JourneyPublicLink> PublicShareLinks => _publicShareLinks.AsReadOnly();

        public Guid UserId { get; private set; }
        public Location StartLocation { get; private set; } = default!;
        public Location EndLocation { get; private set; } = default!;
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TransportType TransportType { get; private set; }
        public double DistanceInKilometers { get; private set; }
        public bool DailyGoalTriggered { get; private set; }

        private Journey() { }

        private Journey(
            JourneyId id,
            Guid userId,
            Location startLocation,
            Location endLocation,
            DateTime startTime,
            DateTime endTime,
            TransportType transportType,
            double distanceInKilometers)
        {
            if (startTime >= endTime) throw new ArgumentException("Start time must be before end time", nameof(startTime));
            if (distanceInKilometers <= 0) throw new ArgumentException("Distance must be greater than zero", nameof(distanceInKilometers));

            Id = id;
            UserId = userId;
            StartLocation = startLocation ?? throw new ArgumentNullException(nameof(startLocation));
            EndLocation = endLocation ?? throw new ArgumentNullException(nameof(endLocation));
            StartTime = startTime;
            EndTime = endTime;
            TransportType = transportType;
            DistanceInKilometers = distanceInKilometers;
            CreatedAt = DateTime.UtcNow;
            DailyGoalTriggered = false;
        }

        public static Journey Create(
            JourneyId id,
            Guid userId,
            Location startLocation,
            Location endLocation,
            DateTime startTime,
            DateTime endTime,
            TransportType transportType,
            double distanceInKilometers)
        {
            var journey = new Journey(id, userId, startLocation, endLocation, startTime, endTime, transportType, distanceInKilometers);
            journey.AddDomainEvent(new JourneyCreatedEvent(journey));
            return journey;
        }

        public void Update(
            Location startLocation,
            Location endLocation,
            DateTime startTime,
            DateTime endTime,
            TransportType transportType,
            double distanceInKilometers)
        {
            if (startLocation == null) throw new ArgumentNullException(nameof(startLocation));
            if (endLocation == null) throw new ArgumentNullException(nameof(endLocation));
            if (startTime == default) throw new ArgumentException("Start time must be provided", nameof(startTime));
            if (endTime == default) throw new ArgumentException("End time must be provided", nameof(endTime));
            if (startTime >= endTime) throw new ArgumentException("Start time must be before end time", nameof(startTime));
            if (distanceInKilometers <= 0) throw new ArgumentException("Distance must be greater than zero", nameof(distanceInKilometers));

            StartLocation = startLocation;
            EndLocation = endLocation;
            StartTime = startTime;
            EndTime = endTime;
            TransportType = transportType;
            DistanceInKilometers = distanceInKilometers;
            LastModified = DateTime.UtcNow;

            AddDomainEvent(new JourneyUpdatedEvent(this));
        }

        public void MarkAsDailyGoalTrigger()
        {
            if (!DailyGoalTriggered)
            {
                DailyGoalTriggered = true;
                LastModified = DateTime.UtcNow;
                AddDomainEvent(new DailyGoalAchievedEvent(UserId, Id, StartTime.Date, DistanceInKilometers));
            }
        }

        public void ShareWithUser(Guid sharedWithUserId)
        {
            if (sharedWithUserId == Guid.Empty)
            {
                throw new ArgumentException("Shared user ID cannot be empty", nameof(sharedWithUserId));
            }

            if (_sharedWithUsers.All(x => x.SharedWithUserId != sharedWithUserId))
            {
                var share = new JourneyShare(Id, sharedWithUserId);
                _sharedWithUsers.Add(share);
                LastModified = DateTime.UtcNow;
                AddDomainEvent(new JourneySharedEvent(Id, UserId, sharedWithUserId));
            }
        }

        public void UnshareWithUser(Guid sharedWithUserId)
        {
            var share = _sharedWithUsers.FirstOrDefault(x => x.SharedWithUserId == sharedWithUserId);
            if (share != null)
            {
                _sharedWithUsers.Remove(share);
                LastModified = DateTime.UtcNow;
                AddDomainEvent(new JourneyUnsharededEvent(Id, UserId, sharedWithUserId));
            }
        }

        public string CreatePublicShareLink()
        {
            var link = $"journey/{Id}/share/{Guid.NewGuid()}";
            var publicLink = new JourneyPublicLink(Id, link);
            _publicShareLinks.Add(publicLink);
            LastModified = DateTime.UtcNow;
            AddDomainEvent(new JourneyPublicShareCreatedEvent(Id, UserId, link));
            return link;
        }

        public void RevokePublicShareLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                throw new ArgumentException("Share link cannot be empty", nameof(link));
            }

            var publicLink = _publicShareLinks.FirstOrDefault(x => x.Link == link && !x.IsRevoked);
            if (publicLink != null)
            {
                publicLink.Revoke();
                LastModified = DateTime.UtcNow;
                AddDomainEvent(new JourneyPublicShareRevokedEvent(Id, UserId, link));
            }
        }
    }

}
