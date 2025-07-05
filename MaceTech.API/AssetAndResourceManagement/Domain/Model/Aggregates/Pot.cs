using System.Text.Json.Serialization;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Commands;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Enums;

namespace MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;

public class Pot
{
    //  @Properties
    [JsonIgnore]
    public long Id { get; set; }
    [JsonIgnore]
    public bool IsUserAssigned { get; set; } = false;
    [JsonIgnore]
    public string Uid { get; set; } = string.Empty;
    [JsonIgnore]
    public bool IsPlantLinked { get; set; } = false;
    [JsonIgnore]
    public long PlantId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    
    public float BatteryLevel { get; set; } = 0.0f;
    public float WaterLevel { get; set; } = 0.0f;
    public float Humidity { get; set; } = 0.0f;
    public float Luminance { get; set; } = 0.0f;
    public float Temperature { get; set; } = 0.0f;
    public float Ph { get; set; } = 0.0f;
    public float Salinity { get; set; } = 0.0f;
    
    public PotStatus Status { get; set; } = PotStatus.Healthy;
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    //  @Constructor
    public Pot() { }
    public Pot(CreatePotCommand command) { }
    
    //  @Methods
    public void AssignUser(PotAssignmentCommand command)
    {
        this.IsUserAssigned = true;
        this.Uid = command.Uid;
        this.Name = command.Name;
        this.Location = command.Location;
        this.AssignedAt = DateTimeOffset.UtcNow;
    }
    public void AssignPlant(long plantId)
    {
        this.IsPlantLinked = true;
        this.PlantId = plantId;
    }
    public void UnassignPlant()
    {
        this.IsPlantLinked = false;
    }
    public void UpdateStatus(PotStatus status)
    {
        this.Status = status;
    }
    public void UpdateMetrics(UpdatePotMetricsCommand command)
    {
        this.BatteryLevel = command.BatteryLevel;
        this.WaterLevel = command.WaterLevel;
        this.Humidity = command.Humidity;
        this.Luminance = command.Luminance;
        this.Temperature = command.Temperature;
        this.Ph = command.Ph;
        this.Salinity = command.Salinity;
    }
    public void Delete()
    {
        this.Status = PotStatus.Deleted;
    }
}