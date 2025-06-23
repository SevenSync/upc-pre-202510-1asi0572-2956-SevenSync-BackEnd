using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Commands;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services.CommandServices;
using MaceTech.API.Analytics.Domain.Services;
using MaceTech.API.Shared.Domain.Repositories;

namespace MaceTech.API.Analytics.Application.Internal.CommandServices;

public class AlertCommandService(
    IAlertRepository alertRepository, 
    IRecommendationGenerationService recommendationService,
    IUnitOfWork unitOfWork) : IAlertCommandService
{
    public async Task<Alert> Handle(CreateAlertCommand command)
    {
        // 1. Usar el servicio de dominio para aplicar las reglas de negocio
        var recommendation = recommendationService.GenerateFromAlert(command.AlertType, command.Value);
        
        // 2. Crear el nuevo agregado 'Alert'
        var alert = new Alert(command.DeviceId, command.AlertType, command.Value, recommendation);

        // 3. Persistir el agregado usando el repositorio
        await alertRepository.AddAsync(alert);
        
        // 4. Confirmar la transacción
        await unitOfWork.CompleteAsync();
        
        return alert;
    }
}