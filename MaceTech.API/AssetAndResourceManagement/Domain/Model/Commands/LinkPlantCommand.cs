namespace MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;

public record LinkPlantCommand(long PotId, string Uid, long PlantId);