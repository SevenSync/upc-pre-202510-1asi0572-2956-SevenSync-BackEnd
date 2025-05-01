using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.ValueObjects.User;
using Domain.IAM.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class UserRepository(MaceTechDataCenterContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(Email email) => await dbContext.Users
        .FirstOrDefaultAsync(u => u.Email == email);
    
    public async Task<User?> GetUserByIdAsync(UserId userId) => await dbContext.Users
        .FirstOrDefaultAsync(u => u.UserId == userId);
    
    public async Task UpdateUserAsync(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }
    public async Task AddUserAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}