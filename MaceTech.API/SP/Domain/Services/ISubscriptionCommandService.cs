using MaceTech.API.SP.Domain.Model.Aggregates;
using MaceTech.API.SP.Domain.Model.Commands;

namespace MaceTech.API.SP.Domain.Services;

public interface ISubscriptionCommandService
{
    public Task<Subscription?> Handle(CreateActiveSubscriptionCommand command);
    public Task<bool> Handle(CancelSubscriptionCommand command);
}