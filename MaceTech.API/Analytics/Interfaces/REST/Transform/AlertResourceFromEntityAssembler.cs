using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Interfaces.REST.Resources;

namespace MaceTech.API.Analytics.Interfaces.REST.Transform;

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert entity)
    {
        return new AlertResource(
            entity.Id, 
            entity.DeviceId, 
            entity.AlertType, 
            entity.TriggerValue, 
            entity.GeneratedRecommendation.Text,
            entity.GeneratedRecommendation.Urgency,
            entity.GeneratedRecommendation.GuideUrl,
            entity.Timestamp
        );
    }
}