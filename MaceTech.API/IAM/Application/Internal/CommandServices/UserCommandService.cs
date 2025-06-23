using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.Shared.Domain.Events;
using MaceTech.API.Shared.Domain.Repositories;
using MediatR;

namespace MaceTech.API.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository, 
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IMediator mediator
    ) : IUserCommandService
{
    public async Task Handle(SignUpCommand command)
    {
        var existingUser = await userRepository.FindByActiveEmailAsync(command.Email);
        if (existingUser != null)
        {
            throw new Exception($"Email {command.Email} already used.");
        }
        
        var user = new User(command.Uid, command.Email);
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while trying to sign up the user.", e);
        }
    }
    
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByActiveEmailAsync(command.Email);
        if (user is null)
            throw new Exception("Invalid email or password.");
        
        var token = tokenService.GenerateToken(user);
        return (user, token);
    }
        
    public async Task Handle(SignOutCommand command)
    {
        var user = await userRepository.FindByUidAsync(command.Uid);
        if (user is null)
            throw new Exception($"User with UID {command.Uid} not found.");
        
        user.IncrementTokenVersion();
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
    }
    
    public async Task Handle(DeleteUserCommand command)
    {
        var user = await userRepository.FindByUidAsync(command.Uid);
        if (user is null)
            throw new Exception($"User with UID {command.Uid} not found.");
        
        user.SetStatusToDeleted();
        await mediator.Publish(new UserDeletedEvent(user.Uid));
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
    }
}