namespace Domain.IAM.Services.Others;

public interface IEncryptService
{
    string Encrypt(string value);
    
    bool Verify(string password, string passwordHashed);
}