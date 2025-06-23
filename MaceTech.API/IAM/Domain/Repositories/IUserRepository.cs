using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByActiveEmailAsync(string token);
    bool ExistsByEmail(string email);
    Task<User?> FindByUidAsync(string uid);
}