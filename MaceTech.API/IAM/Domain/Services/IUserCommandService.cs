using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Commands;

namespace MaceTech.API.IAM.Domain.Services;

public interface IUserCommandService
{
    Task Handle(SignUpCommand command);
    Task<(User user, string token)> Handle(SignInCommand command);
    Task Handle(SignOutCommand command);
    Task Handle(DeleteUserCommand command);
}