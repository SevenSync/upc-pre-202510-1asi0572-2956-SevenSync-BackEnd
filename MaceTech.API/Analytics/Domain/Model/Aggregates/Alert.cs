using MaceTech.API.Analytics.Domain.Model.ValueObjects;

namespace MaceTech.API.Analytics.Domain.Model.Aggregates;

public class Alert
{
    public int Id { get; }
    public string DeviceId { get; private set; }
    public string AlertType { get; private set; }
    public float TriggerValue { get; private set; }
    public Recommendation GeneratedRecommendation { get; private set; }
    public DateTime Timestamp { get; private set; }
    
    public Alert()
    {
        DeviceId = string.Empty;
        AlertType = string.Empty;
        TriggerValue = 0.0f;
        GeneratedRecommendation = new Recommendation(string.Empty, string.Empty, string.Empty);
        Timestamp = DateTime.UtcNow;
    }
    
    public Alert(string deviceId, string alertType, float triggerValue, string recommendationText, string recommendationUrgency, string recommendationGuideUrl)
    {
        DeviceId = deviceId;
        AlertType = alertType;
        TriggerValue = triggerValue;
        GeneratedRecommendation = new Recommendation(recommendationText, recommendationUrgency, recommendationGuideUrl);
        Timestamp = DateTime.UtcNow;
    }
}