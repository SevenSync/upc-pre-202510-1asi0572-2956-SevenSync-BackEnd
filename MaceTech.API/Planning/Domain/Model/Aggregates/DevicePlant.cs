using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Aggregates;

public class DevicePlant
{
    public int Id { get; private set; }
    public string DeviceId { get; private set; }
    
    // Relación con el catálogo de plantas
    public int PlantId { get; private set; }
    public Plant Plant { get; private set; } = null!;

    // Umbrales personalizados por el usuario.
    public OptimalParameters CustomThresholds { get; private set; }

    public DevicePlant() 
    {
        DeviceId = string.Empty;
        CustomThresholds = new OptimalParameters(new Range<double>(0,0), new Range<int>(0,0), new Range<int>(0,0), new Range<double>(0,0), new Range<double>(0,0));
    }
    
    public DevicePlant(string deviceId, Plant plant)
    {
        DeviceId = deviceId;
        Plant = plant;
        PlantId = plant.Id;
        // Por defecto, los umbrales personalizados son los óptimos de la nueva planta
        CustomThresholds = plant.OptimalParameters; 
    }

    /// <summary>
    /// Actualiza la planta asignada a este dispositivo y resetea los umbrales
    /// a los valores óptimos de la nueva planta.
    /// Esta es una regla de negocio del dominio.
    /// </summary>
    /// <param name="newPlant">La nueva entidad Plant del catálogo.</param>
    public void UpdatePlant(Plant newPlant)
    {
        Plant = newPlant;
        PlantId = newPlant.Id;
        // Al cambiar de planta, reseteamos los umbrales a los recomendados por defecto.
        CustomThresholds = newPlant.OptimalParameters;
    }

    // --- MÉTODO RELEVANTE PARA ESTA HU ---
    /// <summary>
    /// Actualiza los umbrales personalizados del dispositivo con nuevos valores.
    /// Incluye lógica de validación para asegurar que los rangos sean lógicos.
    /// </summary>
    /// <param name="newThresholds">El Value Object con los nuevos umbrales.</param>
    public void UpdateCustomThresholds(OptimalParameters newThresholds)
    {
        // Regla de negocio: Validar que el mínimo no sea mayor que el máximo.
        if (newThresholds.TemperaturaAmbiente.Min > newThresholds.TemperaturaAmbiente.Max ||
            newThresholds.Humedad.Min > newThresholds.Humedad.Max ||
            newThresholds.Luminosidad.Min > newThresholds.Luminosidad.Max ||
            newThresholds.SalinidadSuelo.Min > newThresholds.SalinidadSuelo.Max ||
            newThresholds.PhSuelo.Min > newThresholds.PhSuelo.Max)
        {
            throw new ArgumentException("Los valores mínimos de los umbrales no pueden ser mayores que los máximos.");
        }
        
        CustomThresholds = newThresholds;
    }
}