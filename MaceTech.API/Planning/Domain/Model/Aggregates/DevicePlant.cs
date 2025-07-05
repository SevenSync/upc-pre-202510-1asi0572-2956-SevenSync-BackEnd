using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Aggregates;

public class DevicePlant
{
    public int Id { get; private set; }
    public string DeviceId { get; private set; }
    
    public int PlantId { get; private set; }
    public Plant Plant { get; private set; } = null!;

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
        CustomThresholds = plant.OptimalParameters; 
    }

    public void UpdatePlant(Plant newPlant)
    {
        Plant = newPlant;
        PlantId = newPlant.Id;
        CustomThresholds = newPlant.OptimalParameters;
    }

    public void UpdateCustomThresholds(OptimalParameters newThresholds)
    {
        if (newThresholds.TemperaturaAmbiente.Min > newThresholds.TemperaturaAmbiente.Max ||
            newThresholds.Humedad.Min > newThresholds.Humedad.Max ||
            newThresholds.Luminosidad.Min > newThresholds.Luminosidad.Max ||
            newThresholds.SalinidadSuelo.Min > newThresholds.SalinidadSuelo.Max ||
            newThresholds.PhSuelo.Min > newThresholds.PhSuelo.Max)
        {
            throw new ArgumentException("Minium Values cant be greater than maximum values.");
        }
        
        CustomThresholds = newThresholds;
    }
}