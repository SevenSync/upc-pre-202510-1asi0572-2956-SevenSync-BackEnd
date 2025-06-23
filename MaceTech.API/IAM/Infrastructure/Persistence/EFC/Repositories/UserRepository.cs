using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Enums;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public bool ExistsByEmail(string email)
    {
        return Context.Set<User>().Any(user => user.Email.Equals(email));
    }
    
    public async Task<User?> FindByUidAsync(string uid)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(user => user.Uid.Equals(uid));
    }
    
    public async Task<User?> FindByActiveEmailAsync(string token)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(user => user.Email.Equals(token) && user.Status == (int)UserAccountState.Active);
    }
}