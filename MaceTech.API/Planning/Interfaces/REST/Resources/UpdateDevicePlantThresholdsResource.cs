using System.ComponentModel.DataAnnotations;
using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Interfaces.REST.Resources;

public record UpdateDevicePlantThresholdsResource(
    [Required] Range<double> TemperaturaAmbiente,
    [Required] Range<int> Humedad,
    [Required] Range<int> Luminosidad,
    [Required] Range<double> SalinidadSuelo,
    [Required] Range<double> PhSuelo
);