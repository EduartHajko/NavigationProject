using Navigation.Domain.Enums;
using Navigation.Domain.Model.Navigation.Domain.Models;
using Navigation.Domain.Model;
using Navigation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Infrastructure.Extensions
{
    internal static class InitialData
    {
        public static IEnumerable<User> Users => new List<User>
        {
            User.Create(
                UserId.Of(new Guid("b34e7bb2-17b4-48bb-bf2b-65a69f9fc2e1")),
                "eduarthajko1992@gmail.com",
                "Eduart Hajko",
                UserStatus.Active
            ),
            User.Create(
                UserId.Of(new Guid("a2cc86ee-19e6-4a3e-b027-43c1ec9fbb23")),
                "john.doe@gmail.com",
                "John Doe",
                UserStatus.Active
            )
        };

        public static IEnumerable<Journey> Journeys
        {
            get
            {
                var userId1 = new Guid("b34e7bb2-17b4-48bb-bf2b-65a69f9fc2e1");
                var userId2 = new Guid("a2cc86ee-19e6-4a3e-b027-43c1ec9fbb23");

                var journey1 = Journey.Create(
                    JourneyId.Of(Guid.NewGuid()),
                    userId1,
                    new Location(41.3275, 19.8189, "Tirana Start"),
                    new Location(41.3311, 19.8345, "Tirana End"),
                    DateTime.UtcNow.AddHours(-2),
                    DateTime.UtcNow,
                    TransportType.Walking,
                    3.5
                );

                var journey2 = Journey.Create(
                    JourneyId.Of(Guid.NewGuid()),
                    userId2,
                    new Location(51.5072, -0.1276, "London Start"),
                    new Location(51.5079, -0.0877, "London End"),
                    DateTime.UtcNow.AddHours(-1),
                    DateTime.UtcNow,
                    TransportType.Cycling,
                    5.2
                );

                // Share journeys
                journey1.ShareWithUser(userId2);
                journey2.ShareWithUser(userId1);

                return new List<Journey> { journey1, journey2 };
            }
        }

        public static IEnumerable<JourneyShare> JourneyShares =>
            Journeys.SelectMany(j => j.SharedWithUsers);

        public static IEnumerable<JourneyPublicLink> JourneyPublicLinks =>
            Journeys.SelectMany(j => j.PublicShareLinks);
    }
}
