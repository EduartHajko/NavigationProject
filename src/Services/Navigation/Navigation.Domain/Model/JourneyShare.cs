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
        public class JourneyShare : Entity<JourneyShareId>
        {
            public JourneyId JourneyId { get; private set; } = default!;
            public Guid SharedWithUserId { get; private set; }
            public DateTime SharedAt { get; private set; }

            private JourneyShare() { }

            public JourneyShare(JourneyId journeyId, Guid sharedWithUserId)
            {
                JourneyId = journeyId ?? throw new ArgumentNullException(nameof(journeyId));
                SharedWithUserId = sharedWithUserId == Guid.Empty ? throw new ArgumentException("User ID cannot be empty", nameof(sharedWithUserId)) : sharedWithUserId;
                SharedAt = DateTime.UtcNow;
            }
        }
    }

}
