using Administration.Api.Endpoints.Handlers;
using Carter;
using Mapster;
using MassTransit;

namespace Administration.Api.Endpoints
{
    public record JourneyAdminDto(
        Guid Id,
        Guid UserId,
        string TransportType,
        DateTime StartTime,
        DateTime EndTime,
        double DistanceKm
    );

    public record GetAdminJourneysResult(
        List<JourneyAdminDto> Journeys,
        int TotalCount
    );
    public record AdminJourneyRequest(
    Guid? UserId,
    string? TransportType,
    DateTime? StartDateFrom,
    DateTime? StartDateTo,
    DateTime? ArrivalDateFrom,
    DateTime? ArrivalDateTo,
    double? MinDistance,
    double? MaxDistance,
    int Page,
    int PageSize,
    string? OrderBy,
    string? Direction
);

    public class AdminJourneyEndpoints : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/admin/journeys", async (
                [AsParameters] AdminJourneyRequest request,
                IRequestClient<GetAdminJourneysCommand> client,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var query = request.Adapt<GetAdminJourneysCommand>();
                var response = await client.GetResponse<GetAdminJourneysResult>(query, cancellationToken);

                context.Response.Headers.Append("X-Total-Count", response.Message.TotalCount.ToString());
                return Results.Ok(response.Message.Journeys);
            })
            .WithName("GetAdminJourneys")
            .Produces<List<JourneyAdminDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Admin Journeys")
            .WithDescription("Query journeys with filtering, pagination, and sorting for admins.");
        }
    }
}
