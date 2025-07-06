using MaceTech.API.ARM.Domain.Model.Aggregates;
using MaceTech.API.ARM.Domain.Model.Queries;
using MaceTech.API.ARM.Domain.Repositories;
using MaceTech.API.ARM.Domain.Services;

namespace MaceTech.API.ARM.Application.Internal.QueryServices;

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