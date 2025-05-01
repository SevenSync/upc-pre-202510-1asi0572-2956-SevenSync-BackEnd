using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.IAM.CommandServices;
using Domain.IAM.Models.Queries;
using Domain.IAM.Models.ValueObjects.Auth;
using Domain.IAM.Models.ValueObjects.User;
using Domain.IAM.Repositories;
using Domain.IAM.Services.Interfaces;
using Domain.IAM.Services.Others;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.IAM.QueryServices;

public class UserAuthenticationQueryService(
    ISessionRepository sessionRepository,
    IUserRepository userRepository,
    ITokenService tokenService,
    IEncryptService encryptService
    ) : IUserAuthenticationQueryService
{
    private async Task<AuthenticationResult> SaveRefreshTokenRecord(
        long userId,
        string token,
        string refreshToken
    )
    {
        var refreshTokenRecord = new RefreshTokenRecord
        {
            UserId = userId,
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(4),
            Created = DateTime.UtcNow
        };
        
        await sessionRepository.SaveRefreshTokenAsync(refreshTokenRecord);
        
        return new AuthenticationResult
        {
            Token = token,
            RefreshToken = refreshToken,
            Result = true,
            Message = "Refresh Token saved!",
            UserId = userId
        };
    }
    
    //  @Implementations
    public async Task<AuthenticationResult> Handle(GetTokenQuery query)
    {
        var existingUser = await userRepository.GetUserByEmailAsync(new Email(query.Email));
        if (existingUser == null)
        {
            throw new DataException("User doesn't exist!");
        }
        
        if (!encryptService.Verify(query.Password, existingUser.HashPassword.Value))
        {
            throw new DataException("Invalid password or username");
        }
        
        var createdToken = tokenService.GenerateToken(existingUser);
        var createdRefreshToken = UserAuthenticationCommandService.GenerateRefreshToken();
        
        return await this.SaveRefreshTokenRecord(
            existingUser.UserId.Value,
            createdToken,
            createdRefreshToken
        );
    }
}