using Carter;
using MediatR;
using Navigation.Application.Journy.Commands.CreateJourneyPublicLink;
using Navigation.Application.Journy.Commands.FavoriteJourney;
using Navigation.Application.Journy.Commands.RevokePublicLink;
using Navigation.Application.Journy.Commands.ShareJourney;
using Navigation.Application.Journy.Commands.UnfavoriteJourney;

namespace Navigation.Api.Endpoints
{
    public record FavoriteJourneyRequest(Guid UserId);
    public record ShareJourneyRequest(List<Guid> UserIds);
    public record RevokePublicLinkRequest(string Link);
    public record PublicLinkResponse(string Link);

    public class NavigationEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Favorite a journey (idempotent)
            app.MapPost("/journeys/{id:guid}/favorite", async (Guid id, FavoriteJourneyRequest request, ISender sender) =>
            {
                var command = new FavoriteJourneyCommand(id, request.UserId);
               var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("FavoriteJourney")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Favorite Journey")
            .WithDescription("Marks a journey as favorite for the user.");

            // Unfavorite a journey
            app.MapDelete("/journeys/{id:guid}/favorite", async (Guid id,  Guid userId, ISender sender) =>
            {
                var command = new UnfavoriteJourneyCommand(id, userId);
               var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("UnfavoriteJourney")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Unfavorite Journey")
            .WithDescription("Removes a journey from favorites for the user.");

            // Share journey with specific users
            app.MapPost("/journeys/{id:guid}/share", async (Guid id, ShareJourneyRequest request, ISender sender) =>
            {
                var command = new ShareJourneyCommand(id, request.UserIds);
                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("ShareJourney")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Share Journey")
            .WithDescription("Shares a journey with a list of users.");

            // Create public share link
            app.MapPost("/journeys/{id:guid}/public-link", async (Guid id, ISender sender) =>
            {
                var command = new CreateJourneyPublicLinkCommand(id);
                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("CreatePublicLink")
            .Produces<PublicLinkResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Generate Public Link")
            .WithDescription("Generates a public link to access the journey.");

            // Revoke public share link
            app.MapPost("/journeys/{id:guid}/public-link/revoke", async (Guid id, RevokePublicLinkRequest request, ISender sender) =>
            {
                var command = new RevokeJourneyPublicLinkCommand(id, request.Link);
                var result= await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("RevokePublicLink")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Revoke Public Link")
            .WithDescription("Revokes a previously shared public journey link.");
        }
    }

}
