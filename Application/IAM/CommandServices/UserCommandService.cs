using AutoMapper;
using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.Commands;
using Domain.IAM.Models.Enums;
using Domain.IAM.Models.ValueObjects.User;
using Domain.IAM.Repositories;
using Domain.IAM.Services.Interfaces;
using Domain.IAM.Services.Others;
using Shared.Domain.IAM.Exceptions;

namespace Application.IAM.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    IMapper mapper,
    IEncryptService encryptService
    ): IUserCommandService
{
    public async Task Handle(UpdateUserCommand command)
    {
        var user = await userRepository.GetUserByIdAsync(new UserId(command.UserId));
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        
        user.ChangeFullName(command.Name, command.LastName);
        await userRepository.UpdateUserAsync(user);
    }
    
    public async Task<long> Handle(UserRegistrationCommand command)
    {
        var user = mapper.Map<UserRegistrationCommand, User>(command);
        
        var result = await userRepository.GetUserByEmailAsync(user.Email);
        if (result != null)
        {
            throw new UserAlreadyExistsException();
        }

        user.HashPassword = new HashPassword(encryptService.Encrypt(command.Email));
        
        await userRepository.AddUserAsync(user);
        return user.UserId.Value;
    }
}