using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Notifications
{
    public interface IJourneyNotificationService
    {
        Task NotifyJourneyUpdatedAsync(Guid journeyId);
        Task NotifyJourneyDeletedAsync(JourneyId journeyId);
    }
}
