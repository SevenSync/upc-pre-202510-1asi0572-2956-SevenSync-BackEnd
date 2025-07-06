using MaceTech.API.ARM.Domain.Model.Aggregates;
using MaceTech.API.ARM.Domain.Model.Queries;

namespace MaceTech.API.ARM.Domain.Services;

public interface IPotQueryService
{
    public Task<Pot?> Handle(GetPotQuery query);
    public Task<List<Pot>> Handle(GetAllPotsByUserIdQuery query);
}