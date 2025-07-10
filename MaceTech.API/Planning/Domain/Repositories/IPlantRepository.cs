using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Planning.Domain.Repositories;

public interface IPlantRepository : IBaseRepository<Plant>
{
    new Task<IEnumerable<Plant>> ListAsync();
    Task<Plant?> FindByIdAsync(long id);
}