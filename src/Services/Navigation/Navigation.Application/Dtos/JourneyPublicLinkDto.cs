using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Dtos
{
    public record JourneyPublicLinkDto(
     Guid Id,
     Guid JourneyId,
     string Link,
     bool IsRevoked,
     DateTime? RevokedAt,
     DateTime CreatedAt
 );
}
