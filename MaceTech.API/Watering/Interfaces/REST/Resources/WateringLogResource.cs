namespace MaceTech.API.Watering.Interfaces.REST.Resources;

public record WateringLogResource(
    DateTime Fecha,
    int DuracionSegundos,
    double VolumenAguaMl,
    float NivelHumedadInicial,
    float NivelHumedadFinal,
    string Resultado
);