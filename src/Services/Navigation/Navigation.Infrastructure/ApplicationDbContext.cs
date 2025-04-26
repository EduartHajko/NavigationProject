using Microsoft.EntityFrameworkCore;
using Navigation.Application.Data;
using Navigation.Domain.Model;
using Navigation.Domain.Model.Navigation.Domain.Models;
using System.Reflection;


namespace Navigation.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Journey> Journeys => Set<Journey>();
        public DbSet<User> Users =>  Set<User>();
        public DbSet<JourneyPublicLink> JourneyPublicLinks => Set<JourneyPublicLink>();
        public DbSet<JourneyShare> JourneyShares => Set<JourneyShare>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
