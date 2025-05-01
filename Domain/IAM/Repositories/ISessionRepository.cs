using Domain.IAM.Models.Commands;
using Domain.IAM.Models.ValueObjects.Auth;

namespace Domain.IAM.Repositories;

public interface ISessionRepository
{
    public Task<RefreshTokenRecord?> FindExistingRefreshTokenAsync(CreateRefreshTokenCommand command);
    public Task SaveRefreshTokenAsync(RefreshTokenRecord refreshTokenRecord);
}