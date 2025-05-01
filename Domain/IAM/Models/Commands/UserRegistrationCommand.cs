namespace Domain.IAM.Models.Commands;

public record UserRegistrationCommand(
    string Name,
    string LastName,
    string Email,
    string Password
);