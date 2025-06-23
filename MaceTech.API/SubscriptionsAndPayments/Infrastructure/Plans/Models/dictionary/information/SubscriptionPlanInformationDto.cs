namespace MaceTech.API.SubscriptionsAndPayments.Infrastructure.Plans.Models.dictionary.information;

internal class SubscriptionPlanInformationDto
{
    public string LangId { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public string Subtitle { get; set; }
    public List<string> Includes { get; set; } = [];
    public List<string> Restrictions { get; set; } = [];
}