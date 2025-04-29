using Microsoft.EntityFrameworkCore;
using Navigation.Domain.Model;
using Navigation.Domain.Model.Navigation.Domain.Models;

namespace Navigation.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Journey> Journeys { get; }
        DbSet<User> Users { get; }
        DbSet<JourneyPublicLink> JourneyPublicLinks { get; }
        DbSet<JourneyShare> JourneyShares { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
