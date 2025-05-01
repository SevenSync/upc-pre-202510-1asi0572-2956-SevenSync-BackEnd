using Domain.IAM.Models.Aggregates;

namespace Domain.IAM.Services.Others;

public interface ITokenService
{
    string GenerateToken(User user);
    
    Task<int?> ValidateToken(string token);
}