using MaceTech.API.IAM.Domain.Model.Aggregates;

namespace MaceTech.API.IAM.Application.Internal.OutboundServices;

public interface ITokenService
{
    public string GenerateToken(User user);
    public Task<long?> ValidateToken(string token);
}