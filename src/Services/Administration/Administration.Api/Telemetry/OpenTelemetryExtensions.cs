using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Administration.Api.Telemetry
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceName = "Administration.Api";
            var serviceVersion = "1.0.0";

            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(serviceName, serviceVersion)
                    .AddTelemetrySdk()
                    .AddEnvironmentVariableDetector())
                .WithTracing(tracing => tracing
                    // Add ASP.NET Core instrumentation
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, httpRequest) =>
                        {
                            activity.SetTag("http.request.header.x-request-id", httpRequest.Headers["x-request-id"]);
                        };
                        options.EnrichWithHttpResponse = (activity, httpResponse) =>
                        {
                            activity.SetTag("http.response.header.x-response-time", httpResponse.Headers["x-response-time"]);
                        };
                        options.EnrichWithException = (activity, exception) =>
                        {
                            activity.SetTag("exception.message", exception.Message);
                            activity.SetTag("exception.stacktrace", exception.StackTrace);
                        };
                    })
                    // Add HTTP client instrumentation
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithException = (activity, exception) =>
                        {
                            activity.SetTag("exception.message", exception.Message);
                            activity.SetTag("exception.stacktrace", exception.StackTrace);
                        };
                    })
                    // Add MassTransit instrumentation for message broker
                    .AddSource("MassTransit")
                    // Add SQL client instrumentation
                    .AddSqlClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.SetDbStatementForText = true;
                    })
                    // Add Jaeger exporter
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = configuration.GetValue<string>("Jaeger:Host") ?? "localhost";
                        options.AgentPort = configuration.GetValue<int>("Jaeger:Port", 6831);
                    }));

            return services;
        }
    }
}
