using Domain.IAM.Models.Commands;
using Domain.IAM.Models.ValueObjects.Auth;
using Domain.IAM.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IAM;

public class SessionRepository(MaceTechDataCenterContext dbContext) : ISessionRepository
{
    public async Task<RefreshTokenRecord?> FindExistingRefreshTokenAsync(CreateRefreshTokenCommand refreshToken)
    {
        var result = await dbContext.RefreshTokenRecords.
            FirstOrDefaultAsync(r =>
                r.Token == refreshToken.ExpiredToken &&
                r.RefreshToken == refreshToken.RefreshToken &&
                r.UserId == refreshToken.UserId
            );

        return result;
    }
    public async Task SaveRefreshTokenAsync(RefreshTokenRecord refreshTokenRecord)
    {
        await dbContext.RefreshTokenRecords.AddAsync(refreshTokenRecord);
        await dbContext.SaveChangesAsync();
    }
}