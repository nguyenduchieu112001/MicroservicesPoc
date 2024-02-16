using MediatR;
using PricingService.Api.Commands;
using PricingService.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PricingService.Commands;

public class CreateCalculatePriceHandler : IRequestHandler<CreateCalculatePriceCommand, CreateCalculatePriceResult>
{
    private readonly IDataStore dataStore;

    public CreateCalculatePriceHandler(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<CreateCalculatePriceResult> Handle(CreateCalculatePriceCommand request, CancellationToken cancellationToken)
    {

        var existingTariff = await dataStore.Tariffs[request.Code];
        if (existingTariff != null)
        {
            return new CreateCalculatePriceResult();
        }

        var tariff = Tariff.CreateTariff(request.Code);

        var basePremiumCalculationRules = new List<BasePremiumCalculationRule>();
        foreach (var basePremiumCalculationRule in request.BasePremiumCalculationRules)
        {
            basePremiumCalculationRules.Add(new BasePremiumCalculationRule(
                basePremiumCalculationRule.CoverCode, basePremiumCalculationRule.ApplyIfFormula, basePremiumCalculationRule.BasePriceFormula));
        }
        tariff.AddBasePremiumCalculationRules(basePremiumCalculationRules);

        var disCountMarkupRules = new List<DiscountMarkupRule>();
        foreach (var disCountMarkupRule in request.DiscountMarkupRules)
        {
            disCountMarkupRules.Add(new PercentMarkupRule(disCountMarkupRule.ApplyIfFormula, disCountMarkupRule.ParamValue));
        }
        tariff.AddDiscountMarkupRules(disCountMarkupRules);

        await dataStore.Tariffs.Add(tariff);

        await dataStore.CommitChanges();

        return new CreateCalculatePriceResult
        {
            PriceId = tariff.Id
        };
    }
}
