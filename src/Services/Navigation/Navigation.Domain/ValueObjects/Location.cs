using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.ValueObjects
{
    /// <summary>
    /// Represents a geographical location with latitude and longitude
    /// </summary>
    public class Location : IEquatable<Location>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the location</param>
        /// <param name="longitude">The longitude of the location</param>
        /// <param name="name">The name of the location (optional)</param>
        public Location(double latitude, double longitude, string name = null)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
            }

            Latitude = latitude;
            Longitude = longitude;
            Name = name;
        }

        /// <summary>
        /// Gets the latitude of the location
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Gets the longitude of the location
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Gets the name of the location
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        /// <summary>
        /// Determines whether the specified location is equal to the current location.
        /// </summary>
        /// <param name="other">The location to compare with the current location.</param>
        /// <returns>true if the specified location is equal to the current location; otherwise, false.</returns>
        public bool Equals(Location other)
        {
            if (other is null)
            {
                return false;
            }

            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        /// <summary>
        /// Returns the hash code for this location.
        /// </summary>
        /// <returns>A hash code for the current location.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        /// <summary>
        /// Returns a string representation of the location.
        /// </summary>
        /// <returns>A string representation of the location.</returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Name)
                ? $"({Latitude}, {Longitude})"
                : $"{Name} ({Latitude}, {Longitude})";
        }

        /// <summary>
        /// Calculates the distance between two locations in kilometers using the Haversine formula.
        /// </summary>
        /// <param name="other">The other location.</param>
        /// <returns>The distance in kilometers.</returns>
        public double DistanceTo(Location other)
        {
            const double EarthRadiusKm = 6371.0;

            var dLat = ToRadians(other.Latitude - Latitude);
            var dLon = ToRadians(other.Longitude - Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
