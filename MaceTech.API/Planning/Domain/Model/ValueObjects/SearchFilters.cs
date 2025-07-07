namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public class SearchFilters
{
    public string Difficulty { get; set; }
    public List<string> Ubication { get; set; } = new List<string>();
    public string Light { get; set; }
    public string SizePotential { get; set; }
    public List<string> Tags { get; set; } = new List<string>();

    // Constructor sin parámetros requerido por EF Core
    public SearchFilters()
    {
        Difficulty = string.Empty;
        Ubication = new List<string>();
        Light = string.Empty;
        SizePotential = string.Empty;
        Tags = new List<string>();
    }

    public SearchFilters(
        string difficulty,
        List<string> ubication,
        string light,
        string sizePotential,
        List<string> tags)
    {
        Difficulty = difficulty;
        Ubication = ubication;
        Light = light;
        SizePotential = sizePotential;
        Tags = tags;
    }

    public SearchFilters(SearchFilters original)
    {
        Difficulty = original.Difficulty;
        Ubication = original.Ubication;
        Light = original.Light;
        SizePotential = original.SizePotential;
        Tags = original.Tags;
    }
}