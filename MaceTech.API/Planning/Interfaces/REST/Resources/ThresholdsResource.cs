using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Interfaces.REST.Resources;

public record ThresholdsResource(double MinTemp, double MaxTemp, double MinHumidity, double MaxHumidity, double MinLight, double MaxLight, double MinSalinity, double MaxSalinity, double MinPh, double MaxPh);