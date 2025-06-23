using MaceTech.API.Profiles.Domain.Repositories;
using MaceTech.API.Shared.Domain.Events;
using MaceTech.API.Shared.Domain.Repositories;
using MediatR;

namespace MaceTech.API.Profiles.Application.Internal.Handlers;

public class IUserDeletedHandlerEvent(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<UserDeletedEvent> 
{
    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindProfileByUidAsync(notification.Uid);
        if (profile == null) return;
        
        profile.ChangeStatusToDeleted();
        profileRepository.Update(profile);
        await unitOfWork.CompleteAsync();
    }
}