using MaceTech.API.IAM.Domain.Exceptions;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;

namespace MaceTech.API.IAM.Interfaces.ACL.Services;

public class IamContextFacade(
    IUserCommandService userCommandService, 
    IUserQueryService userQueryService
    ) : IIamContextFacade
{
    public async Task<string> CreateUser(string uid, string email, string password)
    {
        var signUpCommand = new SignUpCommand(uid, email, password);
        await userCommandService.Handle(signUpCommand);
        var getUserByUsernameQuery = new GetActiveUserByEmailQuery(email);
        var result = await userQueryService.Handle(getUserByUsernameQuery);

        return result?.Uid ?? "0";
    }

    public async Task<string> FetchUserUidByEmail(string email)
    {
        var getUserByUsernameQuery = new GetActiveUserByEmailQuery(email);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        
        return result?.Uid ?? "0";
    }
    
    public async Task<string> FetchEmailByUserUid(string userUid)
    {
        var getUserByIdQuery = new GetUserByUidQuery(userUid);
        var result = await userQueryService.Handle(getUserByIdQuery);
        
        return result?.Email ?? string.Empty;
    }

    public string GetUserUidFromContext(HttpContext httpContext)
    {
        if (
            !httpContext.Items.TryGetValue("User", out var userDto) ||
            (userDto is not User user))
        {
            throw new InvalidTokenException();
        }
        
        return user.Uid;
    }
}