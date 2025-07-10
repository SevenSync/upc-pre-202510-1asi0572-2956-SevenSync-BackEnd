using System.ComponentModel.DataAnnotations;

namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record CreatePotRecordResource(
    [Required] long DeviceId, 
    [Required] float Temperature, 
    [Required] float Humidity, 
    [Required] float Light, 
    [Required] float Salinity, 
    [Required] float Ph
);