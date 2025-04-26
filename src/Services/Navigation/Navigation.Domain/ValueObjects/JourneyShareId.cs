using Navigation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.ValueObjects
{
    public record JourneyShareId
    {
        public Guid Value { get; }
        private JourneyShareId(Guid value) => Value = value;
        public static JourneyShareId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("JourneyShareId cannot be empty.");
            }

            return new JourneyShareId(value);
        }
    }
}
