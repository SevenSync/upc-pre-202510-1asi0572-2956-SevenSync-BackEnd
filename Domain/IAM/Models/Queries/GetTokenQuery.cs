namespace Domain.IAM.Models.Queries;

public record GetTokenQuery(string Email, string Password);