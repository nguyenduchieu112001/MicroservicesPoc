using PricingService.Api.Queries.Dtos;
using PricingService.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PricingService.Queries.Mappers
{
    public class TariffMapper
    {
        public static IList<BasePremiumCalculationRuleDto> ToBasePremiumDtoList(BasePremiumCalculationRuleList premiumCalculationRules)
        {
            var rules = premiumCalculationRules.GetBasePremiumCalculationRules();
            return rules.Select(rule => new BasePremiumCalculationRuleDto
            {
                CoverCode = rule.CoverCode,
                ApplyIfFormula = rule.ApplyIfFormula,
                BasePriceFormula = rule.BasePriceFormula,
            }).ToList();
        }
        public static IList<DiscountMarkupRuleDto> ToDiscountDtoList(DiscountMarkupRuleList discountMarkupRules)
        {
            var rules = discountMarkupRules.GetDiscountMarkupRules();
            return rules.Select(rule => new DiscountMarkupRuleDto
            {
                ApplyIfFormula = rule.ApplyIfFormula,
                ParamValue = rule.ParamValue,
            }).ToList();
        }
    }
}
