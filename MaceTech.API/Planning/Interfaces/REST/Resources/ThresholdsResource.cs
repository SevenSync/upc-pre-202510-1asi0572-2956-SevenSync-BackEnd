namespace MaceTech.API.Planning.Interfaces.REST.Resources;

public record ThresholdsResource(double MinTemp, double MaxTemp, int MinHumidity, int MaxHumidity);
