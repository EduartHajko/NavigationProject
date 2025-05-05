using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Navigation.Api.Hubs;
using Navigation.Api.NotificationService;
using Navigation.Api.Telemetry;
using Navigation.Application.Notifications;

namespace Navigation.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCarter();
            services.AddSignalR();//Signal R
            services.AddScoped<IJourneyNotificationService, JourneyNotificationService>();
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);
            
            // Add OpenTelemetry services
            services.AddOpenTelemetryServices(configuration);

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            app.MapHub<JourneyHub>("/journeyhub");
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            return app;
        }
    }

}
