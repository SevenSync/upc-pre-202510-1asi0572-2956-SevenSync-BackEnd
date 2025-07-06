using MaceTech.API.SP.Domain.Model.Commands;
using MaceTech.API.SP.Domain.Model.Enums;

namespace MaceTech.API.SP.Domain.Model.Aggregates;

public partial class Subscription
{
    //  |: Properties
    public long Id { get; private set; }
    public string Uid { get; private set; }
    public string SubscriptionId { get; private set; }
    public SubscriptionPlanType Plan { get; private set; }
    public DateTimeOffset ActiveUntil { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset CanceledAt { get; private set; } = DateTimeOffset.MinValue;
    
    //  |: Constructors
    public Subscription()
    {
        Uid = string.Empty;
        Plan = SubscriptionPlanType.Free;
        SubscriptionId = string.Empty;
        
        this.UpdateActiveUntil();
    }

    public Subscription(CreateActiveSubscriptionCommand command)
    {
        Uid = command.Uid;
        Plan = command.Plan;
        SubscriptionId = command.SubscriptionId;
        
        this.UpdateActiveUntil();
    }
    
    //  |: Functions
    public void UpdateActiveUntil()
    {
        ActiveUntil = this.Plan switch
        {
            SubscriptionPlanType.PremiumMonthly => DateTimeOffset.UtcNow.AddMonths(1),
            SubscriptionPlanType.PremiumAnnually => DateTimeOffset.UtcNow.AddYears(1),
            
            _ => ActiveUntil
        };
    }

    public bool IsActive()
    {
        return ActiveUntil > DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        this.CanceledAt = DateTimeOffset.UtcNow;
    }
}