namespace MaceTech.API.IAM.Domain.Model.Commands;

public record SignUpCommand(string Uid, string Email, string Password);