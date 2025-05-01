using Administration.Api.Endpoints;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Administration.Api.Endpoints.Handlers
{
    public record UpdateUserStatusCommand(Guid UserId, UserStatus Status) : ICommand<UpdateUserStatusResponse>;

    public class AdminUserHandler : ICommandHandler<UpdateUserStatusCommand, UpdateUserStatusResponse>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<AdminUserHandler> _logger;

        public AdminUserHandler(
            IPublishEndpoint publishEndpoint,
            ILogger<AdminUserHandler> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<UpdateUserStatusResponse> Handle(UpdateUserStatusCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating user status for user {UserId} to {Status}", command.UserId, command.Status);

                // Publish UserStatusEvent - the consumer will create and publish UserStatusChangedEvent
                var userStatusEvent = new UserStatusEvent(command.UserId, command.Status);
                await _publishEndpoint.Publish(userStatusEvent, cancellationToken);
                
                // In a real implementation, we would:
                // 1. Validate the user exists
                // 2. Update the user status in the database
                // 3. Record the change in an audit table

                return new UpdateUserStatusResponse(true, $"User status updated to {command.Status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user status");
                return new UpdateUserStatusResponse(false, $"Failed to update user status: {ex.Message}");
            }
        }
    }
}
