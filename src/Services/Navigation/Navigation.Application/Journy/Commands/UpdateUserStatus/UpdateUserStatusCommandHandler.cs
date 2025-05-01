using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Navigation.Application.Data;
using Navigation.Domain.Enums;

namespace Navigation.Application.Journy.Commands.UpdateUserStatus
{
    public class UpdateUserStatusCommandHandler(IApplicationDbContext dbContext)
        : ICommandHandler<UpdateUserStatusCommand, UpdateUserStatusResult>
    {
        public async Task<UpdateUserStatusResult> Handle(UpdateUserStatusCommand command, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id.Value == command.UserId, cancellationToken);
            if (user == null)
            {
                return new UpdateUserStatusResult(false);
            }

            user.SetStatus(command.Status);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateUserStatusResult(true);
        }
    }


}
