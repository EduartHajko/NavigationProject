using Navigation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.ValueObjects
{

    public record JourneyId
    {
        public Guid Value { get; }
        private JourneyId(Guid value) => Value = value;
        public static JourneyId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("JourneyId cannot be empty.");
            }

            return new JourneyId(value);
        }
    }
}
