using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;

namespace Administration.Api.Endpoints.Handlers
{
    public record GetAdminJourneysCommand(
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
):ICommand<GetAdminJourneysResult>;
    public record GetAdminJourneysResult(
        List<JourneyAdminDto> Journeys,
        int TotalCount
    );
    public class AdminJourneysCommandHandler(IRequestClient<GetAdminJourneysEvent> client)
       : ICommandHandler<GetAdminJourneysCommand, GetAdminJourneysResult>
    {
        public async Task<GetAdminJourneysResult> Handle(GetAdminJourneysCommand command, CancellationToken cancellationToken)
        {
            // Adapt to the event
            var eventMessage = command.Adapt<GetAdminJourneysEvent>();

            // Request-response over RabbitMQ
            var response = await client.GetResponse<GetAdminJourneysResult>(eventMessage, cancellationToken);

            return response.Message;
        }
    }

}
