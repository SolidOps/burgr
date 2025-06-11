using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Domain.Transients;

namespace SolidOps.UM.Application.UseCases;

public partial class InvitesUseCase
{
    public override async Task<IOpsResult<InviteResult>> CheckInvite(Guid inviteId)
    {
        var invite = await _dependencyInviteRepository.GetSingleById(inviteId);

        if (invite == null)
            return IOpsResult.Invalid("invalid id").ToResult<InviteResult>();

        if (invite.Status != Contracts.Enums.InviteStatusEnum.Sent)
            return IOpsResult.Invalid("invalid invite status").ToResult<InviteResult>();

        return IOpsResult.Ok(new InviteResult()
        {
            Email = invite.Email,
            Creator = invite.CreatorName,
            Message = invite.CreatorMessage
        });
    }

    public override async Task<IOpsResult> UseInvite(Guid inviteId, string password)
    {
        var invite = await _dependencyInviteRepository.GetSingleById(inviteId);

        if (invite == null)
            return IOpsResult.Invalid("invalid id");

        var userCreationService = serviceProvider.GetRequiredService<ISelfUserCreationUseCase>();
        var result = await userCreationService.SafeCreateUser(invite.Email, password);
        if (result.HasError) return result;

        invite.Status = Contracts.Enums.InviteStatusEnum.Used;
        return await _dependencyInviteRepository.Update(invite);
    }
}
