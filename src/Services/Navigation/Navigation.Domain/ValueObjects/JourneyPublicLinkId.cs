using Navigation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.ValueObjects
{
    public record JourneyPublicLinkId
    {
        public Guid Value { get; }
        private JourneyPublicLinkId(Guid value) => Value = value;
        public static JourneyPublicLinkId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("JourneyPublicLinkId cannot be empty.");
            }

            return new JourneyPublicLinkId(value);
        }
    }
}
