namespace MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

public record UnassignPotFromUserCommand(long PotId, string UserUid);