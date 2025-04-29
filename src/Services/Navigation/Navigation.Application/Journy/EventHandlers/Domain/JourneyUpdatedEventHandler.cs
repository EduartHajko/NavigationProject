using MediatR;
using Microsoft.Extensions.Logging;
using Navigation.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.EventHandlers.Domain
{
    public class JourneyUpdatedEventHandler(ILogger<JourneyUpdatedEventHandler> logger)
   : INotificationHandler<JourneyUpdatedEvent>
    {
        public Task Handle(JourneyUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
