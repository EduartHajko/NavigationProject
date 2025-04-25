using Microsoft.EntityFrameworkCore;
using Navigation.Application.Data;
using Navigation.Domain.Model;
using Navigation.Domain.ValueObjects;
using System.Reflection;


namespace Navigation.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Journey> Journeys => Set<Journey>();

        public DbSet<User> Users =>  Set<User>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
