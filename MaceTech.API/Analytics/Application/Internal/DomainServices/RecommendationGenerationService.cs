using MaceTech.API.Analytics.Domain.Model.ValueObjects;
using MaceTech.API.Analytics.Domain.Services;

namespace MaceTech.API.Analytics.Application.Internal.DomainServices;

public class RecommendationGenerationService : IRecommendationGenerationService
{
    public Recommendation GenerateFromAlert(string alertType, float value)
    {
        return alertType.ToLower() switch
        {
            "humedad_baja" => new Recommendation(
                "Regar ahora. Aumentar frecuencia en 30%.",
                "Crítica",
                "/guias/regar"
            ),
            "temperatura_alta" => new Recommendation(
                "Mover a sombra. Temperatura ideal: 18-25°C.",
                "Media",
                "/guias/mover-sombra"
            ),
            "ph_bajo" => new Recommendation(
                "Añadir cal agrícola (1 cucharada por litro de agua).",
                "Crítica",
                "/guias/corregir-ph"
            ),
            // Caso por defecto si llega un tipo de alerta no reconocido
            _ => new Recommendation(
                "Se ha detectado una anomalía no identificada. Revise los parámetros manualmente.",
                "Baja",
                "/guias/diagnostico-general"
            )
        };
    }
}
