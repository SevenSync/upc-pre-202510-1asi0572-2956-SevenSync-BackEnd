namespace Domain.IAM.Models.Commands;

public record RefreshTokenCommand(
    string ExpiredToken,
    string RefreshToken
);