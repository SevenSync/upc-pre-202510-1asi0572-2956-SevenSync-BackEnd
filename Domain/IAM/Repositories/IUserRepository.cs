using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.Commands;
using Domain.IAM.Models.Queries;
using Domain.IAM.Models.ValueObjects;
using Domain.IAM.Models.ValueObjects.Auth;
using Domain.IAM.Models.ValueObjects.User;

namespace Domain.IAM.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(Email email);
    public Task<User?> GetUserByIdAsync(UserId id);
    public Task UpdateUserAsync(User user);
    public Task AddUserAsync(User user);
}