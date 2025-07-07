using MaceTech.API.ARM.Domain.Model.Commands;
using MaceTech.API.ARM.Domain.Model.Constants;
using MaceTech.API.ARM.Domain.Model.Enums;
using MaceTech.API.Shared.Domain.Models.Abstractions;

namespace MaceTech.API.ARM.Domain.Model.Aggregates;

public class Pot : AuditEntity
{
    //  @Properties
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public long PlantId { get; set; } = PotSettings.InvalidPotId;
    
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
    
    //  @Constructors
    public Pot() { }
    public Pot(CreatePotCommand command) { }
    
    //  @Methods
    public void AssignUser(PotAssignmentCommand command)
    {
        this.UserId = command.Uid;
        this.Name = command.Name;
        this.Location = command.Location;
        this.AssignedAt = DateTimeOffset.UtcNow;
    }
    public void UnassignUser()
    {
        this.UserId = string.Empty;
        this.Name = string.Empty;
        this.Location = string.Empty;
        this.AssignedAt = DateTimeOffset.UtcNow;
    }
    public void AssignPlant(long plantId)
    {
        this.PlantId = plantId;
    }
    public void UnassignPlant()
    {
        this.PlantId = PotSettings.InvalidPotId;
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