using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Dtos
{
    public record JourneyDto(
     Guid Id,
     Guid UserId,
     LocationDto StartLocation,
     LocationDto EndLocation,
     DateTime StartTime,
     DateTime EndTime,
     string TransportType,
     double DistanceInKilometers,
     bool DailyGoalTriggered,
     DateTime? CreatedAt,
     DateTime? LastModified
 );
}
