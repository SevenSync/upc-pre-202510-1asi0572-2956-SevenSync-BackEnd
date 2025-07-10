using System.ComponentModel.DataAnnotations;

namespace MaceTech.API.Watering.Interfaces.REST.Resources;

public record CreateWateringLogResource(
    [Required] double DurationSeconds,
    [Required] bool WasSuccessful,
    [Required] string Reason
);