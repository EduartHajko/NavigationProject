using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record GetAdminJourneysEvent(
        Guid? UserId,
        string? TransportType,
        DateTime? StartDateFrom,
        DateTime? StartDateTo,
        DateTime? ArrivalDateFrom,
        DateTime? ArrivalDateTo,
        double? MinDistance,
        double? MaxDistance,
        int Page,
        int PageSize,
        string? OrderBy,
        string? Direction
    ) : IntegrationEvent;
}
