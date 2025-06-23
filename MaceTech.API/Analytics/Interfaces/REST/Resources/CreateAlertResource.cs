using System.ComponentModel.DataAnnotations;

namespace MaceTech.API.Analytics.Interfaces.REST.Resources;

public record CreateAlertResource([Required] string AlertType, [Required] float Value);
