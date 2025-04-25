using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.Enums
{
    /// <summary>
    /// Represents the type of transport used for a journey
    /// </summary>
    public enum TransportType
    {
        /// <summary>
        /// Walking
        /// </summary>
        Walking = 0,

        /// <summary>
        /// Cycling
        /// </summary>
        Cycling = 1,

        /// <summary>
        /// Car
        /// </summary>
        Car = 2,

        /// <summary>
        /// Bus
        /// </summary>
        Bus = 3,

        /// <summary>
        /// Train
        /// </summary>
        Train = 4,

        /// <summary>
        /// Subway
        /// </summary>
        Subway = 5,

        /// <summary>
        /// Tram
        /// </summary>
        Tram = 6,

        /// <summary>
        /// Ferry
        /// </summary>
        Ferry = 7,

        /// <summary>
        /// Airplane
        /// </summary>
        Airplane = 8,

        /// <summary>
        /// Other
        /// </summary>
        Other = 9
    }
}
