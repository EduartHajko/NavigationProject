using Navigation.Domain.Abstractions;
using Navigation.Domain.Enums;
using Navigation.Domain.ValueObjects;

namespace Navigation.Domain.Model
{
    public class Journey : Entity<JourneyId>
    {

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
            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be before end time", nameof(startTime));
            }

            if (distanceInKilometers <= 0)
            {
                throw new ArgumentException("Distance must be greater than zero", nameof(distanceInKilometers));
            }

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

        /// <summary>
        /// Creates a new journey instance
        /// </summary>
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
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }

            ArgumentNullException.ThrowIfNull(startLocation);
            ArgumentNullException.ThrowIfNull(endLocation);

            if (startTime == default)
            {
                throw new ArgumentException("Start time must be provided", nameof(startTime));
            }

            if (endTime == default)
            {
                throw new ArgumentException("End time must be provided", nameof(endTime));
            }

            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be before end time", nameof(startTime));
            }

            if (distanceInKilometers <= 0)
            {
                throw new ArgumentException("Distance must be greater than zero", nameof(distanceInKilometers));
            }

            var journey = new Journey(
                id,
                userId,
                startLocation,
                endLocation,
                startTime,
                endTime,
                transportType,
                distanceInKilometers);

            return journey;
        }

        public Guid UserId { get; private set; }
        public Location StartLocation { get; private set; }
        public Location EndLocation { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TransportType TransportType { get; private set; }
        public double DistanceInKilometers { get; private set; }
        public bool DailyGoalTriggered { get; private set; }
    }

}
