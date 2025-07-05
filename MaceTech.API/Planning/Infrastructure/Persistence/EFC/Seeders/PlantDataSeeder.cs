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
            dto.NombreComun,
            dto.NombreCientifico,
            dto.ImageUrl,
            dto.Descripcion,
            new OptimalParameters(
                new Range<double>(dto.ParametrosOptimos.TemperaturaAmbiente.Min, dto.ParametrosOptimos.TemperaturaAmbiente.Max),
                new Range<int>(dto.ParametrosOptimos.Humedad.Min, dto.ParametrosOptimos.Humedad.Max),
                new Range<int>(dto.ParametrosOptimos.Luminosidad.Min, dto.ParametrosOptimos.Luminosidad.Max),
                new Range<double>(dto.ParametrosOptimos.SalinidadSuelo.Min, dto.ParametrosOptimos.SalinidadSuelo.Max),
                new Range<double>(dto.ParametrosOptimos.PhSuelo.Min, dto.ParametrosOptimos.PhSuelo.Max)
            )
        )).ToList();

        await context.Plants.AddRangeAsync(plants);
        await context.SaveChangesAsync();
        Console.WriteLine($"Seeding completed. Se añadieron {plants.Count} plantas a la base de datos.");
    }
}

file class PlantSeedDto
{
    [JsonPropertyName("nombre_comun")] public string NombreComun { get; set; } = string.Empty;
    [JsonPropertyName("nombre_cientifico")] public string NombreCientifico { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    [JsonPropertyName("parametros_optimos")] public OptimalParametersDto ParametrosOptimos { get; set; } = new();
}
file class OptimalParametersDto
{
    [JsonPropertyName("temperatura_ambiente")] public RangeDto<double> TemperaturaAmbiente { get; set; } = new();
    public RangeDto<int> Humedad { get; set; } = new();
    public RangeDto<int> Luminosidad { get; set; } = new();
    [JsonPropertyName("salinidad_suelo")] public RangeDto<double> SalinidadSuelo { get; set; } = new();
    [JsonPropertyName("ph_suelo")] public RangeDto<double> PhSuelo { get; set; } = new();
}
file class RangeDto<T>
{
    public T Min { get; set; }
    public T Max { get; set; }
}