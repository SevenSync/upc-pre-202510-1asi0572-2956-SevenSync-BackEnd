namespace MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

public record AssignPotToUserCommand(string Uid, long PotId, string Name, string Location);