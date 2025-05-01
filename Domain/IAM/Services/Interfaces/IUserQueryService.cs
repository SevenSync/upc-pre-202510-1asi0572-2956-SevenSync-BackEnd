using Domain.IAM.Models.Aggregates;
using Domain.IAM.Models.Queries;

namespace Domain.IAM.Services.Interfaces;

public interface IUserQueryService
{
    public Task<User?> Handle(GetUserByIdQuery query);
    public Task<User?> Handle(GetUserByEmailQuery query);
}