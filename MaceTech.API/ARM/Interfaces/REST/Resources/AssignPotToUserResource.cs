namespace MaceTech.API.ARM.Interfaces.REST.Resources;

public record AssignPotToUserResource(long PotId, string Name, string Location, int Status);