using System.Text.Json;
using System.Text.Json.Serialization;
using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.ValueObjects;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace MaceTech.API.Planning.Infrastructure.Persistence.EFC.Seeders;

public class PlantDataSeeder
{
    public async Task SeedAsync(AppDbContext context, string jsonFilePath)
    {
        if (context.Plants.Any())
        {
            Console.WriteLine("Plant's table already contains data. Skipping seeding.");
            return;
        }

        Console.WriteLine("Starting seeding with Json file...");
        await using var stream = File.OpenRead(jsonFilePath);
        
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var plantsDto = await JsonSerializer.DeserializeAsync<List<PlantSeedDto>>(stream, options);

        if (plantsDto == null || !plantsDto.Any())
        {
            Console.WriteLine("Error: Couldn't deserialize the JSON or no plants found.");
            return;
        }

        var plants = plantsDto.Select(dto => new Plant(
            dto.CommonName,
            dto.ScientificName,
            dto.ImageUrl,
            dto.Description,
            dto.VisualIdentification.GrowthHabit,
            dto.VisualIdentification.Leaf.Shape,
            dto.VisualIdentification.Leaf.RelativeSize,
            dto.VisualIdentification.Leaf.Texture,
            dto.VisualIdentification.Leaf.Edge,
            dto.VisualIdentification.Leaf.Pattern,
            dto.VisualIdentification.Leaf.MainColors,
            dto.VisualIdentification.Leaf.SecondaryColor,
            dto.VisualIdentification.Flower.Present,
            dto.VisualIdentification.Flower.Color,
            dto.VisualIdentification.Flower.Shape,
            dto.VisualIdentification.Flower.Fragance,
            dto.VisualIdentification.Fruit.Present,
            dto.SearchFilters.Difficulty,
            dto.SearchFilters.Ubication,
            dto.SearchFilters.Light,
            dto.SearchFilters.SizePotential,
            dto.SearchFilters.Tags,
            dto.OptimalParameters.Temperature.Min,
            dto.OptimalParameters.Temperature.Max,
            dto.OptimalParameters.Humidity.Min,
            dto.OptimalParameters.Humidity.Max,
            dto.OptimalParameters.Light.Min,
            dto.OptimalParameters.Light.Max,
            dto.OptimalParameters.Salinity.Min,
            dto.OptimalParameters.Salinity.Max,
            dto.OptimalParameters.Ph.Min,
            dto.OptimalParameters.Ph.Max
            )).ToList();

        await context.Plants.AddRangeAsync(plants);
        await context.SaveChangesAsync();
        Console.WriteLine($"Seeding completed. Se añadieron {plants.Count} plantas a la base de datos.");
    }
}

file class PlantSeedDto
{
    [JsonPropertyName("nombre_comun")] public string CommonName { get; set; } = string.Empty;
    [JsonPropertyName("nombre_cientifico")] public string ScientificName { get; set; } = string.Empty;
    [JsonPropertyName("imageUrl")] public string ImageUrl { get; set; } = string.Empty;
    [JsonPropertyName("descripcion")] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("identificacion_visual")] public VisualIdentificationDto VisualIdentification { get; set; } = new();
    [JsonPropertyName("filtros_busqueda")] public SearchFiltersDto SearchFilters { get; set; } = new();

    [JsonPropertyName("parametros_optimos")] public OptimalParametersDto OptimalParameters { get; set; } = new();

}
file class VisualIdentificationDto
{
    [JsonPropertyName("habito_crecimiento")] public string GrowthHabit { get; set; } = "";
    [JsonPropertyName("hoja")] public LeafDto Leaf { get; set; } = new();
    [JsonPropertyName("flor")] public FlowerDto Flower { get; set; } = new();
    [JsonPropertyName("fruto")] public FruitDto Fruit { get; set; } = new();
}

file class LeafDto
{
    [JsonPropertyName("forma")] public string Shape { get; set; } = string.Empty;
    [JsonPropertyName("tamano_relativo")] public string RelativeSize { get; set; } = string.Empty;
    [JsonPropertyName("textura")] public List<string> Texture { get; set; } = new List<string>();
    [JsonPropertyName("borde")] public string Edge { get; set; } = string.Empty;
    [JsonPropertyName("patron")] public string Pattern { get; set; } = string.Empty;
    [JsonPropertyName("color_principal")] public string MainColors { get; set; } = string.Empty;
    [JsonPropertyName("colores_secundarios")] public List<string> SecondaryColor { get; set; } = new List<string>();
}

file class FlowerDto
{
    [JsonPropertyName("presente")] public bool Present { get; set; } = false;
    [JsonPropertyName("color")] public List<string> Color { get; set; } = new List<string>();
    [JsonPropertyName("forma")] public string Shape { get; set; } = string.Empty;
    [JsonPropertyName("fragancia")] public bool Fragance { get; set; } = false;
}
file class FruitDto
{
    [JsonPropertyName("presente")] public bool Present { get; init; } = false;
}
file class SearchFiltersDto
{
    [JsonPropertyName("dificultad")] public string Difficulty { get; set; } = "";
    [JsonPropertyName("ubicacion")] public List<string> Ubication { get; set; } = new List<string>();
    [JsonPropertyName("luz")] public string Light { get; set; } = "";
    [JsonPropertyName("tamano_potencial")] public string SizePotential { get; set; } = "";
    [JsonPropertyName("tags")] public List<string> Tags { get; set; } = new List<string>();
}
file class OptimalParametersDto
{
    [JsonPropertyName("temperatura_ambiente")] public RangeDto<double> Temperature { get; set; } = new();
    [JsonPropertyName("humedad")] public RangeDto<int> Humidity { get; set; } = new();
    [JsonPropertyName("luminosidad")] public RangeDto<int> Light { get; set; } = new();
    [JsonPropertyName("salinidad_suelo")] public RangeDto<double> Salinity { get; set; } = new();
    [JsonPropertyName("ph_suelo")] public RangeDto<double> Ph { get; set; } = new();
}
file class RangeDto<T>
{
    public T Min { get; set; }
    public T Max { get; set; }
}