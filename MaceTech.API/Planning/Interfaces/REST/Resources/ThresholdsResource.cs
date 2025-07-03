namespace MaceTech.API.Planning.Interfaces.REST.Resources;

// Recurso anidado para los umbrales
public record ThresholdsResource(double MinTemp, double MaxTemp, int MinHumidity, int MaxHumidity);
