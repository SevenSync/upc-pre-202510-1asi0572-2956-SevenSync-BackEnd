using MaceTech.API.SP.Domain.Model.Enums;

namespace MaceTech.API.SP.Domain.Model.Commands;

public record CreateActiveSubscriptionCommand(string Uid, SubscriptionPlanType Plan, string SubscriptionId);