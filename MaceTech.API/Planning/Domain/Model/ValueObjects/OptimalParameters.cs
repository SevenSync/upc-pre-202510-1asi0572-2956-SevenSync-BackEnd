namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public record Range<T>(T Min, T Max);

public class OptimalParameters
{
    public Range<double> Temperature { get; set; }
    public Range<double> Humidity { get; set; }
    public Range<double> Light { get; set; }
    public Range<double> Salinity { get; set; }
    public Range<double> Ph { get; set; }

    // Constructor sin parámetros requerido por EF Core
    public OptimalParameters()
    {
        Humidity = new Range<double>(0, 0);
        Temperature = new Range<double>(0, 0);
        Light = new Range<double>(0, 0);
        Salinity = new Range<double>(0, 0);
        Ph = new Range<double>(0, 0);
    }

    public OptimalParameters(
        double temperatureMin,
        double temperatureMax,
        double humidityMin,
        double humidityMax,
        double lightMin,
        double lightMax,
        double salinityMin,
        double salinityMax,
        double phMin,
        double phMax)
    {
        Temperature = new Range<double>(temperatureMin, temperatureMax);
        Humidity = new Range<double>(humidityMin, humidityMax);
        Light = new Range<double>(lightMin, lightMax);
        Salinity = new Range<double>(salinityMin, salinityMax);
        Ph = new Range<double>(phMin, phMax);
    }

    public OptimalParameters(OptimalParameters original)
    {
        Temperature = original.Temperature;
        Humidity = original.Humidity;
        Light = original.Light;
        Salinity = original.Salinity;
        Ph = original.Ph;
    }
}