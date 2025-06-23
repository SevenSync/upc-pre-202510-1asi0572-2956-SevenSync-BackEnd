using MaceTech.API.SubscriptionsAndPayments.Infrastructure.Plans.Models.dictionary;

namespace MaceTech.API.SubscriptionsAndPayments.Infrastructure.Plans.Models;

internal class SubscriptionsPlansDto
{
    public List<SubscriptionPlanDictionaryDto> Plans { get; set; } = [];
}