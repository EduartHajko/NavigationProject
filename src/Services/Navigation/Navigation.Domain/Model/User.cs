using Navigation.Domain.Abstractions;
using Navigation.Domain.Enums;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Domain.Model
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User : Entity<UserId>
    {

        private User(UserId id, string email, string displayName, UserStatus status)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Display name cannot be empty.", nameof(displayName));
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Email = email;
            DisplayName = displayName;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public static User Create(UserId id, string email, string displayName, UserStatus status)
        {
            return new User(id, email, displayName, status);
        }

        /// <summary>
        /// Gets the user email
        /// </summary>
        public string Email { get; private set; } = default!;

        /// <summary>
        /// Gets the user display name
        /// </summary>
        public string DisplayName { get; private set; } = default!;

        /// <summary>
        /// Gets the user status
        /// </summary>
        public UserStatus Status { get; private set; }
    }

}
