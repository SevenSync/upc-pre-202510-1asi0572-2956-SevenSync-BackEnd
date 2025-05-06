namespace MaceTech.API.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    public Task<long> CreateUser(string email, string password);
    public Task<long> FetchUserIdByEmail(string email);
    public Task<string> FetchEmailByUserId(long userId);
}