using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record GetMonthlyStatisticsEvent(
        int Page,
        int PageSize,
        string? OrderBy = null
    ) : IntegrationEvent;
}
