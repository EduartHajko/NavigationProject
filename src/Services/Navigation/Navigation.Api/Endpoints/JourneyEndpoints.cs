using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;
using Navigation.Application.Dtos;
using Navigation.Application.Journy.Commands.CreateJourny;
using Navigation.Application.Journy.Commands.DeleteJourny;
using Navigation.Application.Journy.Commands.UpdateJourny;
using Navigation.Application.Journy.Queries.GetJourny;

namespace Navigation.Api.Endpoints;

public record CreateJourneyRequest(JourneyDto JourneyDto);
public record CreateJourneyResponse(Guid Id);

public record UpdateJourneyRequest(JourneyDto JourneyDto);
public record UpdateJourneyResponse(bool IsSuccess);

public record DeleteJourneyRequest(Guid JourneyId);
public record DeleteJourneyResponse(bool IsSuccess);

public record GetJourneysResponse(PaginatedResult<JourneyDto> Journeys);

public class JourneyEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Create Journey
        app.MapPost("/journeys", async (CreateJourneyRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateJournyCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateJourneyResponse>();
            return Results.Created($"/journeys/{response.Id}", response);
        })
        .WithName("CreateJourney")
        .Produces<CreateJourneyResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Journey")
        .WithDescription("Creates a new journey with provided details.");

        // Update Journey
        app.MapPut("/journeys", async (UpdateJourneyRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateJournyCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateJourneyResponse>();
            return Results.Ok(response);
        })
        .WithName("UpdateJourney")
        .Produces<UpdateJourneyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Journey")
        .WithDescription("Updates an existing journey.");

        // Delete Journey
        app.MapDelete("/journeys/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteJourneyCommand(id);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteJourneyResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteJourney")
        .Produces<DeleteJourneyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Journey")
        .WithDescription("Deletes a journey by ID.");

        // Get Paginated Journeys
        app.MapGet("/journeys", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var query = new GetJourneysQuery(request);
            var result = await sender.Send(query);
            var response = result.Adapt<GetJourneysResponse>();
            return Results.Ok(response);
        })
        .WithName("GetJourneys")
        .Produces<GetJourneysResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Journeys")
        .WithDescription("Retrieves journeys in paginated format.");
    }
}




