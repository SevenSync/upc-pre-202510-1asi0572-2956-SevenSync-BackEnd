using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Planning.Infrastructure.Persistence.EFC.Repositories;

public class PlantRepository(AppDbContext context) : BaseRepository<Plant>(context), IPlantRepository
{
    public new async Task<IEnumerable<Plant>> ListAsync()
    {
        return await Context.Set<Plant>().ToListAsync();
    }

    public async Task<Plant?> FindByIdAsync(long id)
    {
        return await Context.Set<Plant>().FindAsync(id);
    }
}