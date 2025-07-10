using System.ComponentModel.DataAnnotations;

namespace MaceTech.API.Planning.Interfaces.REST.Resources;

public record AssignPlantToDeviceResource([Required] long PlantId);
