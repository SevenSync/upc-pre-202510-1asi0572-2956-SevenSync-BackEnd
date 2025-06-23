namespace MaceTech.API.IAM.Domain.Exceptions;

public class InvalidTokenException() : Exception($"The provided token is invalid or has expired.");