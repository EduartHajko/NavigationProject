using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Navigation.Api.Hubs
{
    public class JourneyHub : Hub
    {

        public Task JoinJourneyGroup(Guid journeyId) =>
            Groups.AddToGroupAsync(Context.ConnectionId, journeyId.ToString());

        public Task LeaveJourneyGroup(Guid journeyId) =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, journeyId.ToString());
    }
}
