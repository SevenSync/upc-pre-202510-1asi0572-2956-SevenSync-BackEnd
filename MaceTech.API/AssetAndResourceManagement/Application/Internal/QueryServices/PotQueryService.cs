using MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Queries;
using MaceTech.API.AssetAndResourceManagement.Domain.Repositories;
using MaceTech.API.AssetAndResourceManagement.Domain.Services;

namespace MaceTech.API.AssetAndResourceManagement.Application.Internal.QueryServices;

public class PotQueryService(
    IPotRepository repository
    ) : IPotQueryService
{
    public async Task<Pot?> Handle(GetPotQuery query)
    {
        return await repository.FindPotByIdAsync(query.PotId);
    }
    public async Task<List<Pot>> Handle(GetAllPotsByUserIdQuery query)
    {
        return await repository.FindPotsByUserIdAsync(query.Uid);
    }
}