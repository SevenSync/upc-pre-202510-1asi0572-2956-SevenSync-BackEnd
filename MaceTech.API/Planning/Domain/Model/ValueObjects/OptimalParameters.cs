namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

/// <summary>
/// Representa un rango numérico con un valor mínimo y máximo.
/// </summary>
/// <typeparam name="T">El tipo de dato del rango (double, int, etc.)</typeparam>
public record Range<T>(T Min, T Max);

/// <summary>
/// Value Object que encapsula todos los parámetros ambientales óptimos para una planta.
/// </summary>
public record OptimalParameters(
    Range<double> TemperaturaAmbiente,
    Range<int> Humedad,
    Range<int> Luminosidad,
    Range<double> SalinidadSuelo,
    Range<double> PhSuelo
);