using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Navigation.Application.Data;
using Navigation.Application.Journy.Commands.UpdateUserStatus;
using Navigation.Domain.Enums;

namespace Navigation.Application.Journy.EventHandlers.Integration
{
    public class UserStatusEventConsumer(ISender sender, ILogger<UserStatusEventConsumer> logger)
        : IConsumer<UserStatusEvent>
    {
        public async Task Consume(ConsumeContext<UserStatusEvent> context)
        {
            var @event = context.Message;

            logger.LogInformation("UserStatusEvent received for user {UserId} with status {Status}", @event.UserId, @event.Status);
            var domainStatus = (Navigation.Domain.Enums.UserStatus)(int)@event.Status;
            var command = new UpdateUserStatusCommand(@event.UserId, domainStatus);

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                logger.LogWarning("Failed to update status for user {UserId}", @event.UserId);
            }
        }
    }

}
