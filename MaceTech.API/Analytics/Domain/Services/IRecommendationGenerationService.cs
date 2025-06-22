using MaceTech.API.Analytics.Domain.Model.ValueObjects;

namespace MaceTech.API.Analytics.Domain.Services;

public interface IRecommendationGenerationService
{
    Recommendation GenerateFromAlert(string alertType, float value);
}