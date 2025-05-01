using Administration.Api.Endpoints.Handlers;
using Carter;
using Mapster;
using MassTransit;

namespace Administration.Api.Endpoints
{
    public record MonthlyDistanceDto(
        Guid UserId,
        int Year,
        int Month,
        double TotalDistanceKm
    );

    public record GetMonthlyDistanceResult(
        List<MonthlyDistanceDto> Statistics,
        int TotalCount
    );

    public record MonthlyDistanceRequest(
        int Page = 1,
        int PageSize = 10,
        string? OrderBy = null
    );

    public class AdminStatisticsEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/admin/statistics/monthly-distance", async (
                [AsParameters] MonthlyDistanceRequest request,
                IRequestClient<GetMonthlyStatisticsCommand> client,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var query = request.Adapt<GetMonthlyStatisticsCommand>();
                var response = await client.GetResponse<GetMonthlyDistanceResult>(query, cancellationToken);

                context.Response.Headers.Append("X-Total-Count", response.Message.TotalCount.ToString());
                return Results.Ok(response.Message.Statistics);
            })
            .WithName("GetMonthlyDistanceStatistics")
            .Produces<List<MonthlyDistanceDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Monthly Distance Statistics")
            .WithDescription("Retrieves total distance travelled per user per calendar month for admin analysis.");
        }
    }
}
