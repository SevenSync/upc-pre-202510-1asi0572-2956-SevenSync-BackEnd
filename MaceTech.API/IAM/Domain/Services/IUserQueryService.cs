using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Queries;

namespace MaceTech.API.IAM.Domain.Services;

public interface IUserQueryService
{
    public Task<User?> Handle(GetUserByUidQuery query);
    public Task<User?> Handle(GetActiveUserByEmailQuery query);
    public Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
}