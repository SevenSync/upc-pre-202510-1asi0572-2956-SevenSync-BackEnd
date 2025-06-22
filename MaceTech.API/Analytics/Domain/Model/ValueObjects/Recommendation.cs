namespace MaceTech.API.Analytics.Domain.Model.ValueObjects;

// Value Object para encapsular la recomendación generada
public record Recommendation(string Text, string Urgency, string GuideUrl);