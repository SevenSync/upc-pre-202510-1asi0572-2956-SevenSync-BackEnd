namespace Domain.IAM.Models.Commands;

public record UpdateUserCommand(
    int UserId,
    string Name,
    string LastName
);