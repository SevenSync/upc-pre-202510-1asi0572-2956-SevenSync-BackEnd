namespace MaceTech.API.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    public Task<string> CreateUser(string uid, string email, string password);
    public Task<string> FetchUserUidByEmail(string email);
    public Task<string> FetchEmailByUserUid(string userUid);
    public string GetUserUidFromContext(HttpContext httpContext);
}