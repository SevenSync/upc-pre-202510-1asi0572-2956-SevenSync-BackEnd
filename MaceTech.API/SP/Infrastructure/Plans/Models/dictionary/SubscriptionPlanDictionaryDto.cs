using MaceTech.API.SP.Infrastructure.Plans.Models.dictionary.information;

namespace MaceTech.API.SP.Infrastructure.Plans.Models.dictionary;

internal class SubscriptionPlanDictionaryDto
{
    public string Sku { get; set; }
    public List<SubscriptionPlanInformationDto> Dictionary { get; set; } = [];
}