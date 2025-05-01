using Domain.IAM.Models.Commands;
using Domain.IAM.Models.ValueObjects;

namespace Domain.IAM.Services.Interfaces;

public interface IUserCommandService
{
    public Task Handle(UpdateUserCommand command);
    public Task<long> Handle(UserRegistrationCommand command);
}