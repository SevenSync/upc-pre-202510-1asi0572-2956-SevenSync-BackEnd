using MaceTech.API.AssetAndResourceManagement.Domain.Model.Queries;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class GetPotQueryFromResponseAssembler
{
    public static GetPotQuery ToQueryFromResource(long potId)
    {
        return new GetPotQuery(potId);
    }
}