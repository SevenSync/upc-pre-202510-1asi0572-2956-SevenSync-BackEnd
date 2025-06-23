using MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.AssetAndResourceManagement.Domain.Repositories;

public interface IPotRepository : IBaseRepository<Pot>
{
    public Task<Pot?> FindPotByIdAsync(long id);
    public Task<List<Pot>> FindPotsByUserIdAsync(string uid);
}