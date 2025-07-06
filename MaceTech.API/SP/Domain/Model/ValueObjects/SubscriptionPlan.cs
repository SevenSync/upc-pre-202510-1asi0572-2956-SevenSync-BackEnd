namespace MaceTech.API.SP.Domain.Model.ValueObjects;

public record SubscriptionPlan(
    string Sku, 
    string Name,
    string Tag,
    string Subtitle,
    List<string> Includes,
    List<string> Restrictions
);