using MaceTech.API.IAM.Domain.Model.Commands;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Services;

namespace MaceTech.API.IAM.Interfaces.ACL.Services;

public class IamContextFacade(
    IUserCommandService userCommandService, 
    IUserQueryService userQueryService
    ) : IIamContextFacade
{
    public async Task<long> CreateUser(string username, string password)
    {
        var signUpCommand = new SignUpCommand(username, password);
        await userCommandService.Handle(signUpCommand);
        var getUserByUsernameQuery = new GetUserByEmailQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);

        return result?.Id ?? 0;
    }

    public async Task<long> FetchUserIdByEmail(string email)
    {
        var getUserByUsernameQuery = new GetUserByEmailQuery(email);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        
        return result?.Id ?? 0;
    }
    
    public async Task<string> FetchEmailByUserId(long userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        
        return result?.Email ?? string.Empty;
    }
}