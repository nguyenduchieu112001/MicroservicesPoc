using MediatR;
using PricingService.Api.Commands;
using PricingService.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PricingService.Commands
{
    public class UpdateCalculatePriceHandler : IRequestHandler<UpdateCalculatePriceCommand, UpdateCalculatePriceResult>
    {
        private readonly IDataStore dataStore;
        public UpdateCalculatePriceHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        public async Task<UpdateCalculatePriceResult> Handle(UpdateCalculatePriceCommand request, CancellationToken cancellationToken)
        {
            var tariff = await dataStore.Tariffs[request.Code];
            if (tariff == null)
            {
                return new UpdateCalculatePriceResult();
            }

            var basePremiumRules = tariff.BasePremiumRules.GetBasePremiumCalculationRules();
            tariff.ClearBasePremiumCalculationRules(basePremiumRules);

            var discountMarkupRules = tariff.DiscountMarkupRules.GetDiscountMarkupRules();
            tariff.ClearDiscountMarkupRules(discountMarkupRules);

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

            await dataStore.Tariffs.Update(tariff);

            await dataStore.CommitChanges();

            return new UpdateCalculatePriceResult
            {
                PriceId = tariff.Id
            };
        }
    }
}
