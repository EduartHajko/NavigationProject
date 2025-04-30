using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Navigation.Application.Notifications;
using Navigation.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.EventHandlers.Domain
{
    public class JourneyUpdatedEventHandler(IJourneyNotificationService notifier)
       : INotificationHandler<JourneyUpdatedEvent>
    {
        public Task Handle(JourneyUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return notifier.NotifyJourneyUpdatedAsync(notification.Journey.Id.Value);
        }
    }
}
