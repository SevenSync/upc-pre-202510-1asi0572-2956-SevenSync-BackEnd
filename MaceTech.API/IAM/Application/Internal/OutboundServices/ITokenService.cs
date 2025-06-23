using MaceTech.API.IAM.Domain.Model.Aggregates;

namespace MaceTech.API.IAM.Application.Internal.OutboundServices;

public interface ITokenService
{
    public string GenerateToken(User user);
    public Task<string?> ValidateToken(string token);
}