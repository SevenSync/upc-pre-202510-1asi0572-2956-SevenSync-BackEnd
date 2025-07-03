using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Aggregates;

public class Plant(
    string commonName,
    string scientificName,
    string imageUrl,
    string description,
    OptimalParameters optimalParameters)
{
    public int Id { get; private set; }
    public string CommonName { get; private set; } = commonName;
    public string ScientificName { get; private set; } = scientificName;
    public string ImageUrl { get; private set; } = imageUrl;
    public string Description { get; private set; } = description;

    // La entidad Plant "posee" un Value Object de parámetros óptimos.
    public OptimalParameters OptimalParameters { get; private set; } = optimalParameters;

    // Constructor vacío requerido por Entity Framework Core para la materialización.
    public Plant() : this(string.Empty, string.Empty, string.Empty, string.Empty, new OptimalParameters(new Range<double>(0,0), new Range<int>(0,0), new Range<int>(0,0), new Range<double>(0,0), new Range<double>(0,0)))
    {
    }
}