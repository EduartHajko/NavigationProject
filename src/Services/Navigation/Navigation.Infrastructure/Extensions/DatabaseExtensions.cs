using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Navigation.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync(); 

            await SeedAsync(context);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedUsersAsync(context);
            await SeedJourneysAsync(context);
            await SeedJourneySharesAsync(context);
            await SeedJourneyPublicLinksAsync(context);
        }

        private static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(InitialData.Users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedJourneysAsync(ApplicationDbContext context)
        {
            if (!await context.Journeys.AnyAsync())
            {
                await context.Journeys.AddRangeAsync(InitialData.Journeys);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedJourneySharesAsync(ApplicationDbContext context)
        {
            if (!await context.JourneyShares.AnyAsync())
            {
                await context.JourneyShares.AddRangeAsync(InitialData.JourneyShares);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedJourneyPublicLinksAsync(ApplicationDbContext context)
        {
            if (!await context.JourneyPublicLinks.AnyAsync())
            {
                await context.JourneyPublicLinks.AddRangeAsync(InitialData.JourneyPublicLinks);
                await context.SaveChangesAsync();
            }
        }
    }

}
