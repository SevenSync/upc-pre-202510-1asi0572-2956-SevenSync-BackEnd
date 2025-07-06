namespace MaceTech.API.ARM.Domain.Model.Commands;

public record UnassignPotFromUserCommand(long PotId, string UserUid);