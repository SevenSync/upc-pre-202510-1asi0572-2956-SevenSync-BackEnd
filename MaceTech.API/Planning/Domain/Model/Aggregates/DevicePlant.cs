using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Aggregates;

public class DevicePlant
{
    public int Id { get; private set; }
    public string DeviceId { get; private set; }
    public int PlantId { get; private set; }
    public Plant Plant { get; private set; } = null!;
    
    public DevicePlant() 
    {
        DeviceId = string.Empty;
    }
    
    public DevicePlant(string deviceId, Plant plant)
    {
        DeviceId = deviceId;
        Plant = plant;
        PlantId = plant.Id;
    }

    public void UpdatePlant(Plant newPlant)
    {
        Plant = newPlant;
        PlantId = newPlant.Id;
    }

    public void UpdateCustomThresholds(OptimalParameters newThresholds)
    {
        if (newThresholds.Temperature.Min > newThresholds.Temperature.Max ||
            newThresholds.Humidity.Min > newThresholds.Humidity.Max ||
            newThresholds.Light.Min > newThresholds.Light.Max ||
            newThresholds.Salinity.Min > newThresholds.Salinity.Max ||
            newThresholds.Ph.Min > newThresholds.Ph.Max)
        {
            throw new ArgumentException("Minium Values cant be greater than maximum values.");
        }
        
    }
}