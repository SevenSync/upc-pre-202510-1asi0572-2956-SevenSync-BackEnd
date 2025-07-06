using MaceTech.API.ARM.Domain.Model.Queries;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class GetPotQueryFromResponseAssembler
{
    public static GetPotQuery ToQueryFromResource(long potId)
    {
        return new GetPotQuery(potId);
    }
}