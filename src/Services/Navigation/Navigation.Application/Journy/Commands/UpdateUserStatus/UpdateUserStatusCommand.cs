using BuildingBlocks.CQRS;
using Navigation.Domain.Enums;

namespace Navigation.Application.Journy.Commands.UpdateUserStatus
{
    public record UpdateUserStatusCommand(Guid UserId, UserStatus Status)
     : ICommand<UpdateUserStatusResult>;

    public record UpdateUserStatusResult(bool IsSuccess);
}
