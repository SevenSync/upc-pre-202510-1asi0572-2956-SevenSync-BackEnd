using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.Queries;
using Domain.IAM.Models.ValueObjects.User;
using Domain.IAM.Repositories;
using Domain.IAM.Services.Interfaces;

namespace Application.IAM.QueryServices;

public class UserQueryService(
    IUserRepository userRepository
    ) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        if (query.Id <= 0)
        {
            throw new Exception("UserId is invalid.");
        }
        
        return await userRepository.GetUserByIdAsync(new UserId(query.Id));
    }
    
    public async Task<User?> Handle(GetUserByEmailQuery query)
    {
        return await userRepository.GetUserByEmailAsync(new Email(query.Email));
    }
}