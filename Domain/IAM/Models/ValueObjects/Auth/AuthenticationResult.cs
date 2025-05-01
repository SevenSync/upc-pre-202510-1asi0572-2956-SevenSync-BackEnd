using Shared.Domain.IAM;

namespace Domain.IAM.Models.ValueObjects.Auth;

public class AuthenticationResult
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool Result { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public long UserId { get; set; } = UserConstraints.InvalidUserId;
}