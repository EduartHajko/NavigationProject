using Microsoft.AspNetCore.SignalR;
using Navigation.Api.Hubs;
using Navigation.Application.Notifications;
using Navigation.Domain.ValueObjects;

namespace Navigation.Api.NotificationService
{
    public class JourneyNotificationService(IHubContext<JourneyHub> hubContext) : IJourneyNotificationService
    {
        public Task NotifyJourneyUpdatedAsync(Guid journeyId)
        {
            return hubContext.Clients
                .Group(journeyId.ToString())
                .SendAsync("JourneyUpdated", journeyId);
        }

        public Task NotifyJourneyDeletedAsync(JourneyId journeyId)
        {
            return hubContext.Clients
                .Group(journeyId.ToString())
                .SendAsync("JourneyDeleted", journeyId);
        }
    }

}
