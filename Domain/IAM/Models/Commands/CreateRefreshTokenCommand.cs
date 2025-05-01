using Shared.Domain.IAM;

namespace Domain.IAM.Models.Commands;

public class CreateRefreshTokenCommand
{
    public string ExpiredToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int UserId { get; set; } = UserConstraints.InvalidUserId;
}