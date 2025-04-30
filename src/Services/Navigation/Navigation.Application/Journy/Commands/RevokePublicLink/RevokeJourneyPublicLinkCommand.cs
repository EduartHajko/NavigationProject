using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Commands.RevokePublicLink
{
    public record RevokeJourneyPublicLinkCommand(Guid JourneyId, string Link) : ICommand<bool>;

}
