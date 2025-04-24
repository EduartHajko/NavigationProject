using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record UserStatusEvent: IntegrationEvent
    {
        public Guid UserId { get; set; } = default!;
    }
}
