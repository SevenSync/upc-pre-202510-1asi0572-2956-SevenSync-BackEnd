using MaceTech.API.SP.Application.External.Sku.Requests;
using MaceTech.API.SP.Domain.Model.Enums;

namespace MaceTech.API.SP.Application.External.Sku.Services;

public interface ISkuAndPriceIdConverter
{
    public string Convert(ConvertPriceIdToSkuRequest request);
    public string Convert(ConvertSkuToPriceIdRequest request);
    public SubscriptionPlanType Convert(ConvertSkuToSubscriptionPlanType sku);
}