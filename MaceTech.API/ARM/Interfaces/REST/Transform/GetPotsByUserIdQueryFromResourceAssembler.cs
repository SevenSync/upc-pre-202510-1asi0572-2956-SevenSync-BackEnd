using MaceTech.API.ARM.Domain.Model.Queries;

namespace MaceTech.API.ARM.Interfaces.REST.Transform;

public static class GetPotsByUserIdQueryFromResourceAssembler
{
    public static GetAllPotsByUserIdQuery ToQueryFromResource(string uid)
    {
        return new GetAllPotsByUserIdQuery(uid);
    }
}