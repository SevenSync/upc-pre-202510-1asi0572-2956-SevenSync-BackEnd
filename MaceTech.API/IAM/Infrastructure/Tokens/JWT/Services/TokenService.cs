using System.Security.Claims;
using System.Text;
using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace MaceTech.API.IAM.Infrastructure.Tokens.JWT.Services;

public class TokenService(
    IOptions<TokenSettings> tokenSettings, 
    IUserRepository userRepository
    ) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;
    private readonly JsonWebTokenHandler _tokenHandler = new JsonWebTokenHandler();

    public string GenerateToken(User user)
    {
        var secret = this._tokenSettings.Secret;
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Sid, user.Uid),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Version, user.TokenVersion.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }

    public async Task<string?> ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }
        
        var key     = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
        var handler = new JsonWebTokenHandler();
        var result  = await handler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(key),
            ValidateIssuer           = false,
            ValidateAudience         = false,
            ValidateLifetime         = true,
            ClockSkew                = TimeSpan.Zero
        });

        if (!result.IsValid)
        {
            return null;
        }
        
        //  |: Version and Uic
        var jwtToken = (JsonWebToken)result.SecurityToken;
        var sidClaim  = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        var verClaim  = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Version)?.Value;
        if (!int.TryParse(verClaim, out var tokenVersion) || (sidClaim == null))
        {
            return null;
        }

        //  |: Compare
        var user = await userRepository.FindByUidAsync(sidClaim);
        if (user == null || user.TokenVersion != tokenVersion)
        {
            return null;
        }

        return user.Uid;
    }
}