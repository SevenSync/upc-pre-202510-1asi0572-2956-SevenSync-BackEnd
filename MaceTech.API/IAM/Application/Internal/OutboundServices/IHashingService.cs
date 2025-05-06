namespace MaceTech.API.IAM.Application.Internal.OutboundServices;

public interface IHashingService
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashedPassword);
}