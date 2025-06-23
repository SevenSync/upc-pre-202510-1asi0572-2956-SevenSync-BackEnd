namespace MaceTech.API.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(string Uid, string Email, string Token);