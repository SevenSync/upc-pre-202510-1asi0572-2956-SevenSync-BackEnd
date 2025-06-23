namespace MaceTech.API.SubscriptionsAndPayments.Domain.Model.ValueObjects;

public record SubscriptionPlan(
    string Sku, 
    string Name,
    string Tag,
    string Subtitle,
    List<string> Includes,
    List<string> Restrictions
);