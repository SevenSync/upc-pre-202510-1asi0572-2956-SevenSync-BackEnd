using MaceTech.API.Planning.Domain.Model.ValueObjects;

namespace MaceTech.API.Planning.Domain.Model.Aggregates;

public class Plant
{
    public int Id { get; }
    public string CommonName { get; private set; }
    public string ScientificName { get; private set; }
    public string ImageUrl { get; private set; }
    public string Description { get; private set; }
    public VisualIdentification VisualIdentification { get; private set; }
    public SearchFilters SearchFilters { get; private set; }
    public OptimalParameters OptimalParameters { get; private set; }

    public Plant()
    {
        CommonName = string.Empty;
        ScientificName = string.Empty;
        ImageUrl = string.Empty;
        Description = string.Empty;
        VisualIdentification = new VisualIdentification();
        SearchFilters = new SearchFilters();
        OptimalParameters = new OptimalParameters();
    }

    public Plant(
        string commonName,
        string scientificName,
        string imageUrl,
        string description,
        string growthHabit,
        string leafShape,
        string leafRelativeSize,
        List<string> leafTexture,
        string leafEdge,
        string leafPattern,
        string leafMainColors,
        List<string> leafSecondaryColor,
        bool flowerPresent,
        List<string> flowerColor,
        string flowerShape,
        bool flowerFragance,
        bool fruitPresent,
        string difficulty,
        List<string> ubication,
        string light,
        string sizePotential,
        List<string> tags,
        double temperatureMin,
        double temperatureMax,
        double humidityMin,
        double humidityMax,
        double lightMin,
        double lightMax,
        double salinityMin,
        double salinityMax,
        double phMin,
        double phMax
        )
    {
        CommonName = commonName;
        ScientificName = scientificName;
        ImageUrl = imageUrl;
        Description = description;
        VisualIdentification = new VisualIdentification(growthHabit, leafShape, leafRelativeSize, leafTexture, leafEdge,
            leafPattern, leafMainColors, leafSecondaryColor, flowerPresent, flowerColor, flowerShape, flowerFragance,
            fruitPresent);
        SearchFilters = new SearchFilters(difficulty, ubication , light, sizePotential, tags);
        OptimalParameters = new OptimalParameters(temperatureMin, temperatureMax, humidityMin, humidityMax, lightMin,
            lightMax, salinityMin, salinityMax, phMin, phMax);
    }
}