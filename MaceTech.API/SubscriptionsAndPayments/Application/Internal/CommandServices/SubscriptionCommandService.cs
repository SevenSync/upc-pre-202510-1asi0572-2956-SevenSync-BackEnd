using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Aggregates;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Commands;
using MaceTech.API.SubscriptionsAndPayments.Domain.Repositories;
using MaceTech.API.SubscriptionsAndPayments.Domain.Services;

namespace MaceTech.API.SubscriptionsAndPayments.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository repository,
    IUnitOfWork unitOfWork
    ): ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateActiveSubscriptionCommand command)
    {
        //  > Check to create a subscription for the user.
        var existingSubscription = await repository.FindSubscriptionByUserIdAndTypeAsync(
            command.Uid, command.Plan);
        if (existingSubscription != null)
        {
            try
            {
                existingSubscription.UpdateActiveUntil();
                repository.Update(existingSubscription);
                await unitOfWork.CompleteAsync();
                return existingSubscription;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        //  > Create a new subscription for the user.
        var subscription = new Subscription(command);
        try
        {
            await repository.AddAsync(subscription);
            await unitOfWork.CompleteAsync();
            return subscription;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public async Task<bool> Handle(CancelSubscriptionCommand command)
    {
        //  > Check to find the subscription for the user.
        var existingSubscription = await repository.FindActiveSubscriptionByUidAsync(command.Uid);
        if (existingSubscription == null)
        {
            return false;
        }
        
        //  > Cancel the subscription.
        existingSubscription.Cancel();
        repository.Update(existingSubscription);
        
        try
        {
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}