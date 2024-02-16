using System.Collections.Generic;
using System.Linq;

namespace PricingService.Domain;

public class BasePremiumCalculationRuleList
{
    private readonly List<BasePremiumCalculationRule> rules;

    public BasePremiumCalculationRuleList(List<BasePremiumCalculationRule> rules)
    {
        this.rules = rules;
    }

    public void AddBasePriceRule(string coverCode, string applyIfFormula, string basePriceFormula)
    {
        rules.Add(new BasePremiumCalculationRule(coverCode, applyIfFormula, basePriceFormula));
    }

    public void RemoveBasePriceRule(BasePremiumCalculationRule item)
    {
        rules.Remove(item);
    }

    public List<BasePremiumCalculationRule> GetBasePremiumCalculationRules()
    {
        return new List<BasePremiumCalculationRule>(rules);
    }

    public decimal CalculateBasePriceFor(Cover cover, Calculation calculation)
    {
        return rules
            .Where(r => r.Applies(cover, calculation))
            .Select(r => r.CalculateBasePrice(calculation))
            .FirstOrDefault();
    }
}