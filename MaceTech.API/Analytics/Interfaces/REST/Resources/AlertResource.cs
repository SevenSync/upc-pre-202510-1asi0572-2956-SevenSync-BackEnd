namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record AlertResource(int Id, string DeviceId, string AlertType, float TriggerValue, string Recommendation, string Urgency, string GuideUrl, DateTime Timestamp);