using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.Analytics.Domain.Model.Commands;

namespace MaceTech.API.Analytics.Domain.Services.CommandServices;

public interface IAlertCommandService
{
    Task<Alert> Handle(CreateAlertCommand command);
}