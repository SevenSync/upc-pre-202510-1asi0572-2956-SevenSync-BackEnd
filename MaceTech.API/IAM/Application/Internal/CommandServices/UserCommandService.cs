using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace MaceTech.API.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository, 
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork
    ) : IUserCommandService
{
    public async Task Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByEmail(command.Email))
            throw new Exception($"Email {command.Email} already used.");
        
        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Email, hashedPassword);
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
        var user = await userRepository.FindByEmailAsync(command.Email);
        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid email or password.");
        
        var token = tokenService.GenerateToken(user);
        return (user, token);
    }
}