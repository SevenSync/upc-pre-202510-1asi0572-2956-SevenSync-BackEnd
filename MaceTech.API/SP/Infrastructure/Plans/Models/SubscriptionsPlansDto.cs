using MaceTech.API.SP.Infrastructure.Plans.Models.dictionary;

namespace MaceTech.API.SP.Infrastructure.Plans.Models;

internal class SubscriptionsPlansDto
{
    public List<SubscriptionPlanDictionaryDto> Plans { get; set; } = [];
}