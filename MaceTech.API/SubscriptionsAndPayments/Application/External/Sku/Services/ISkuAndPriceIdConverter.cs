using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Requests;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Enums;

namespace MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Services;

public interface ISkuAndPriceIdConverter
{
    public string Convert(ConvertPriceIdToSkuRequest request);
    public string Convert(ConvertSkuToPriceIdRequest request);
    public SubscriptionPlanType Convert(ConvertSkuToSubscriptionPlanType sku);
}