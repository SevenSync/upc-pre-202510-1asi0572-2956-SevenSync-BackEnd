using Domain.IAM.Models.Commands;
using Domain.IAM.Models.ValueObjects.Auth;

namespace Domain.IAM.Services.Interfaces;

public interface IUserAuthenticationCommandService
{
    public Task<AuthenticationResult> Handle(CreateRefreshTokenCommand command);
}