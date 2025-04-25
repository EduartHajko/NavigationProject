using Navigation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.ValueObjects
{
    public record UserId
    {
        public Guid Value { get; }
        private UserId(Guid value) => Value = value;
        public static UserId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("UserId cannot be empty.");
            }

            return new UserId(value);
        }
    }
}
