using System.Text.Json;
using MaceTech.API.SP.Domain.Model.ValueObjects;
using MaceTech.API.SP.Domain.Repositories;
using MaceTech.API.SP.Infrastructure.Plans.Models;

namespace MaceTech.API.SP.Infrastructure.Plans.Repository;

public class JsonSubscriptionPlansRepository : ISubscriptionPlansRepository
{
    //  |: Variables
    private readonly Dictionary<string, List<SubscriptionPlan>> _plansByLanguage;
        
    //  |: Constructor
    public JsonSubscriptionPlansRepository(IHostEnvironment environment)
    {
        var root = environment.ContentRootPath;
        var filePath = Path.Combine(
            root,
            "SP",
            "Infrastructure",
            "Plans",
            "Content",
            "subscription-plans.json"
        );

        var json = File.ReadAllBytes(filePath);
        var dto = JsonSerializer.Deserialize<SubscriptionsPlansDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var plans = dto?.Plans ?? [];
        this._plansByLanguage = plans
            .SelectMany(p => p.Dictionary.Select(d => (p.Sku, d)))
            .GroupBy(x => x.d.LangId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g
                    .Select(x => new SubscriptionPlan(
                        x.Sku,
                        x.d.Name,
                        x.d.Tag,
                        x.d.Subtitle,
                        x.d.Includes,
                        x.d.Restrictions))
                    .ToList(),
                StringComparer.OrdinalIgnoreCase
            );
    }
    
    //  |: Functions
    public List<SubscriptionPlan> GetPlans(string language)
    {
        return this._plansByLanguage.TryGetValue(language, out var list) ? list : [];
    }
}