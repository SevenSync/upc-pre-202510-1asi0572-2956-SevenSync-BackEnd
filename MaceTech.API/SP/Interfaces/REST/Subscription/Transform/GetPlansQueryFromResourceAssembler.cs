using MaceTech.API.SP.Domain.Model.Queries;

namespace MaceTech.API.SP.Interfaces.REST.Subscription.Transform;

public static class GetPlansQueryFromResourceAssembler
{
    public static GetPlansQuery ToQueryFromResource(string language)
    {
        return new GetPlansQuery(language);
    }
}