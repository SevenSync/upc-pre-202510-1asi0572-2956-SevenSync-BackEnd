namespace MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

public record AssignPotToUserCommand(long PotId, string Uid, string Name, string Location);