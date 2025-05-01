using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.IAM.Models.Commands;
using Domain.IAM.Models.ValueObjects.Auth;
using Domain.IAM.Repositories;
using Domain.IAM.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.IAM.CommandServices;

public class UserAuthenticationCommandService(
    ISessionRepository sessionRepository,
    IConfiguration configuration
    ): IUserAuthenticationCommandService
{
    private string GenerateToken(int userId)
    {
        //  Key is always defined in the [[appsettings.json]] file.
        var key = configuration.GetValue<string>("JwtSettings:key");
        var keyBytes = Encoding.ASCII.GetBytes(key!);

        var claims = new ClaimsIdentity();
        claims.AddClaim(
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()
            )
        );

        var credentialsToken = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = credentialsToken
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

        var createdToken = tokenHandler.WriteToken(tokenConfig);
        return createdToken;
    }
    public static string GenerateRefreshToken()
    {
        var byteArray = new byte[64];
        var refreshToken = "";

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(byteArray);
        refreshToken = Convert.ToBase64String(byteArray);

        return refreshToken;
    }
    
    private async Task<AuthenticationResult> SaveRefreshTokenRecord(
        int userId,
        string token,
        string refreshToken
    )
    {
        var refreshTokenRecord = new RefreshTokenRecord
        {
            UserId = userId,
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(3),
            Created = DateTime.UtcNow
        };
        
        await sessionRepository.SaveRefreshTokenAsync(refreshTokenRecord);
        
        return new AuthenticationResult
        {
            Token = token,
            RefreshToken = refreshToken,
            Result = true,
            Message = "Refresh Token saved!"
        };
    }
    public async Task<AuthenticationResult> Handle(CreateRefreshTokenCommand command)
    {
        var foundRefreshToken = await sessionRepository.FindExistingRefreshTokenAsync(command);

        if (foundRefreshToken == null)
        {
            return new AuthenticationResult
            {
                Result = false,
                Message = "Invalid Refresh Token"
            };
        }
        
        var createdRefreshToken = GenerateRefreshToken();
        var createdToken = GenerateToken(command.UserId);

        return await this.SaveRefreshTokenRecord(command.UserId, createdToken, createdRefreshToken);
    }
}