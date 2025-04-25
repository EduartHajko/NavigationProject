using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.Enums
{
    /// <summary>
    /// Represents the status of a user
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// Active user
        /// </summary>
        Active = 0,

        /// <summary>
        /// Suspended user
        /// </summary>
        Suspended = 1,

        /// <summary>
        /// Deactivated user
        /// </summary>
        Deactivated = 2
    }
}
