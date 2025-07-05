namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public record Range<T>(T Min, T Max);

public record OptimalParameters(
    Range<double> TemperaturaAmbiente,
    Range<int> Humedad,
    Range<int> Luminosidad,
    Range<double> SalinidadSuelo,
    Range<double> PhSuelo
);