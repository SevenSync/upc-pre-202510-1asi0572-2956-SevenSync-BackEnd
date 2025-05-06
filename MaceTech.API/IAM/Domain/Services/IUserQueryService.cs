using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Queries;

namespace MaceTech.API.IAM.Domain.Services;

public interface IUserQueryService
{
    public Task<User?> Handle(GetUserByIdQuery query);
    public Task<User?> Handle(GetUserByEmailQuery query);
    public Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
}