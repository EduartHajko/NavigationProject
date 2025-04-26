using Navigation.Domain.Abstractions;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.Model
{


    namespace Navigation.Domain.Models
    {
        public class JourneyPublicLink : Entity<JourneyPublicLinkId>
        {
            public JourneyId JourneyId { get; private set; } = default!;
            public string Link { get; private set; } = default!;
            public bool IsRevoked { get; private set; }
            public DateTime? RevokedAt { get; private set; }
            public JourneyPublicLink(JourneyId journeyId, string link)
            {
                JourneyId = journeyId ?? throw new ArgumentNullException(nameof(journeyId));
                Link = link ?? throw new ArgumentNullException(nameof(link));
                CreatedAt = DateTime.UtcNow;
                IsRevoked = false;
            }

            public void Revoke()
            {
                if (!IsRevoked)
                {
                    IsRevoked = true;
                    RevokedAt = DateTime.UtcNow;
                }
            }
        }
    }

}
