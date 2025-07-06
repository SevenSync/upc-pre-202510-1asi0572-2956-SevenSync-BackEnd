namespace MaceTech.API.ARM.Domain.Model.Commands;

public record PotAssignmentCommand(long PotId, string Uid, string Name, string Location);