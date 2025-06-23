using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Queries;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.IAM.Domain.Services;

namespace MaceTech.API.IAM.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByUidQuery query)
    {
        return await userRepository.FindByUidAsync(query.UserUid);
    }
    public async Task<User?> Handle(GetActiveUserByEmailQuery query)
    {
        return await userRepository.FindByActiveEmailAsync(query.Email);
    } 
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await userRepository.ListAsync();
    }
}
