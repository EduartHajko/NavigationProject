using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Dtos
{
    public class JourneyAdminDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string TransportType { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public double DistanceInKilometers { get; init; }

        public JourneyAdminDto(Guid id, Guid userId, string transportType, DateTime start, DateTime end, double distance)
        {
            Id = id;
            UserId = userId;
            TransportType = transportType;
            StartTime = start;
            EndTime = end;
            DistanceInKilometers = distance;
        }
    }
}
