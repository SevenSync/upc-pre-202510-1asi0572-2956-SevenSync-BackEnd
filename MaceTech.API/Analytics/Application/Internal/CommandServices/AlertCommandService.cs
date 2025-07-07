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
        var recommendation = recommendationService.GenerateFromAlert(command.AlertType, command.Value);
        
        var alert = new Alert(command.DeviceId, command.AlertType, command.Value, recommendation.Text, recommendation.Urgency, recommendation.GuideUrl);

        await alertRepository.AddAsync(alert);
        
        await unitOfWork.CompleteAsync();
        
        return alert;
    }
}