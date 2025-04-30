using MediatR;
using Navigation.Application.Notifications;
using Navigation.Domain.Events;

namespace Navigation.Application.Journy.EventHandlers.Domain
{
 
    public class JourneyDeletedEventHandler(IJourneyNotificationService notifier)
   : INotificationHandler<JourneyDeletedEvent>
    {
        public Task Handle(JourneyDeletedEvent notification, CancellationToken cancellationToken)
        {
            return notifier.NotifyJourneyDeletedAsync(notification.JourneyId);
        }
    }
}
