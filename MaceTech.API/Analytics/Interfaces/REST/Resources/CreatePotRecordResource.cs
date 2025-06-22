using System.ComponentModel.DataAnnotations;

namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record CreatePotRecordResource(
    [Required] string DeviceId, 
    [Required] float Temperature, 
    [Required] float Humidity, 
    [Required] int Light, 
    [Required] float Salinity, 
    [Required] float Ph
);