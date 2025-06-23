using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Requests;
using MaceTech.API.SubscriptionsAndPayments.Application.External.Sku.Services;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Enums;

namespace MaceTech.API.SubscriptionsAndPayments.Infrastructure.Sku;

public class SkuAndStipePriceConverter : ISkuAndPriceIdConverter
{
    //  |: Variables
    private readonly IDictionary<string, string> _priceIdBySku = new Dictionary<string, string>
    {
        ["free_plan"] = "missing_str",
        ["premium_plan_monthly"] = "price_1RV079Pw8agxSozrFXsH9vut",
        ["premium_plan_yearly"] = "price_1RV07pPw8agxSozrljWWyCYu"
    };
    private readonly IDictionary<string, string> _skuByPriceId;
    
    //  |: Constructor
    public SkuAndStipePriceConverter()
    {
        this._skuByPriceId = this._priceIdBySku
            .ToDictionary(
                kvp => kvp.Value,
                kvp => kvp.Key,
                StringComparer.OrdinalIgnoreCase
            );
    }
    
    //  |: Functions
    public string Convert(ConvertPriceIdToSkuRequest request)
    {
        var priceId = request.PriceId;
        if (!this._skuByPriceId.TryGetValue(priceId, out var sku))
        {
            throw new ArgumentException($"PriceId '{priceId}' is not recognized.");
        }

        return sku;
    }

    public string Convert(ConvertSkuToPriceIdRequest request)
    {
        var sku = request.Sku;
        if (!this._priceIdBySku.TryGetValue(sku, out var priceId))
        {
            throw new ArgumentException($"SKU '{sku}' is not recognized.");
        }
        
        return priceId;
    }

    public SubscriptionPlanType Convert(ConvertSkuToSubscriptionPlanType sku)
    {
        return sku.Sku switch
        {
            "premium_plan_monthly" => SubscriptionPlanType.PremiumMonthly,
            "premium_plan_yearly" => SubscriptionPlanType.PremiumAnnually,
            
            _ => SubscriptionPlanType.Free
        };
    }
}