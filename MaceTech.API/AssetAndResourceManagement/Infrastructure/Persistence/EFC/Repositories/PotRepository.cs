using MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;
using MaceTech.API.AssetAndResourceManagement.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.AssetAndResourceManagement.Infrastructure.Persistence.EFC.Repositories;

public class PotRepository(AppDbContext context) : BaseRepository<Pot>(context), IPotRepository
{
    public async Task<Pot?> FindPotByIdAsync(long id)
    {
        return await Context.Set<Pot>().FindAsync(id);
    }
    public async Task<List<Pot>> FindPotsByUserIdAsync(string uid)
    {
        return await Context.Set<Pot>()
            .Where(pot => pot.Uid == uid)
            .ToListAsync();
    }
}