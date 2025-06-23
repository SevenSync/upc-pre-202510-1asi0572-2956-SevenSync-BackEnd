using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Enums;

namespace MaceTech.API.SubscriptionsAndPayments.Domain.Model.Commands;

public record CreateActiveSubscriptionCommand(string Uid, SubscriptionPlanType Plan, string SubscriptionId);