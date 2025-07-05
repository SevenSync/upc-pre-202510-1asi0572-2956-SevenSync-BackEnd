using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Services;

namespace MaceTech.API.Analytics.Application.Internal.DomainServices;

public class RecommendationGenerationService : IRecommendationGenerationService
{
    public Recommendation GenerateFromAlert(string alertType, float value)
    {
        return alertType.ToLower() switch
        {
            "low_humidity" => new Recommendation(
                "Watering now. Add 30% to frequency.",
                "Critical",
                "/guides/water"
            ),
            "high_temperature" => new Recommendation(
                "Move to a shadow. Ideal temperature: 18-25°C.",
                "Media",
                "/guides/move-to-shadow"
            ),
            "low_ph" => new Recommendation(
                "Add agricultural lime (1 tablespoon per liter of water)",
                "Critical",
                "/guides/control-ph"
            ),
            _ => new Recommendation(
                "There was an anomaly detected. Check the parameter manually.",
                "Low",
                "/guides/general-diagnosis"
            )
        };
    }
}
