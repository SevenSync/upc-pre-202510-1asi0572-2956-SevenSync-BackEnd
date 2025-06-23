using MaceTech.API.AssetAndResourceManagement.Domain.Model.Queries;

namespace MaceTech.API.AssetAndResourceManagement.Interfaces.REST.Transform;

public static class GetPotsByUserIdQueryFromResourceAssembler
{
    public static GetAllPotsByUserIdQuery ToQueryFromResource(string uid)
    {
        return new GetAllPotsByUserIdQuery(uid);
    }
}