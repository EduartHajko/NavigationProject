using Administration.Api.Endpoints.Handlers;
using BuildingBlocks.Messaging.Events;
using Carter;
using Mapster;
using MassTransit;

namespace Administration.Api.Endpoints
{
    public record UpdateUserStatusRequest(UserStatus Status);
    public record UpdateUserStatusResponse(bool Success, string Message);

    public class AdminUserEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/admin/users/{id:guid}/status", async (
                Guid id,
                UpdateUserStatusRequest request,
                IRequestClient<UpdateUserStatusCommand> client,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateUserStatusCommand(id, request.Status);
                var response = await client.GetResponse<UpdateUserStatusResponse>(command, cancellationToken);
                
                if (response.Message.Success)
                {
                    return Results.Ok(response.Message);
                }
                
                return Results.BadRequest(response.Message);
            })
            .WithName("UpdateUserStatus")
            .Produces<UpdateUserStatusResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update User Status")
            .WithDescription("Allows admins to change a user's account status (Active, Suspended, or Deactivated).");
        }
    }
}
