using Domain.IAM.Models.Queries;
using Domain.IAM.Models.ValueObjects.Auth;

namespace Domain.IAM.Services.Interfaces;

public interface IUserAuthenticationQueryService
{
    public Task<AuthenticationResult> Handle(GetTokenQuery query);
}