using Microsoft.EntityFrameworkCore;
using Navigation.Domain.Model;

namespace Navigation.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Journey> Journeys { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
