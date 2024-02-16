using System.Collections.Generic;

namespace PricingService.Api.Queries.Dtos
{
    public class TariffDto
    {
        public string Code { get; set; }
        public IList<BasePremiumCalculationRuleDto> BasePremiumCalculationRules { get; set; }
        public IList<DiscountMarkupRuleDto> DiscountMarkupRules { get; set; }
    }
}
