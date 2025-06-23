namespace MaceTech.API.IAM.Domain.Exceptions;

public class EmailAlreadyInUseException(string email) : Exception($"The email '{email}' is already in use.");