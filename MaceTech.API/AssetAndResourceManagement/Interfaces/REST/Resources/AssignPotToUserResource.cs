namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Resources;

public record AssignPotToUserResource(long PotId, string Name, string Location, int Status);