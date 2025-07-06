namespace MaceTech.API.ARM.Domain.Model.Commands;

public record LinkPlantCommand(long PotId, string Uid, long PlantId);