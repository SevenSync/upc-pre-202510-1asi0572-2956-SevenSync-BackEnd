using MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Queries;

namespace MaceTech.API.AssetAndResourceManagement.Domain.Services;

public interface IPotQueryService
{
    public Task<Pot?> Handle(GetPotQuery query);
    public Task<List<Pot>> Handle(GetAllPotsByUserIdQuery query);
}