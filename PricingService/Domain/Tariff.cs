using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PricingService.Domain;

public class Tariff
{
    [JsonProperty] private List<BasePremiumCalculationRule> basePremiumRules;

    [JsonProperty] private List<DiscountMarkupRule> discountMarkupRules;

    private Tariff(string code)
    {
        Id = Guid.NewGuid();
        Code = code;
        basePremiumRules = new List<BasePremiumCalculationRule>();
        discountMarkupRules = new List<DiscountMarkupRule>();
    }
    public Tariff(Guid id, string code)
    {
        Id = id;
        Code = code;
        basePremiumRules = new List<BasePremiumCalculationRule>();
        discountMarkupRules = new List<DiscountMarkupRule>();
    }
    public Guid Id { get; }
    public string Code { get; }

    [JsonIgnore] public BasePremiumCalculationRuleList BasePremiumRules => new(basePremiumRules);

    [JsonIgnore] public DiscountMarkupRuleList DiscountMarkupRules => new(discountMarkupRules);

    public Calculation CalculatePrice(Calculation calculation)
    {
        CalcBasePrices(calculation);
        ApplyDiscounts(calculation);
        UpdateTotals(calculation);
        return calculation;
    }


    private void CalcBasePrices(Calculation calculation)
    {
        foreach (var cover in calculation.Covers.Values)
            cover.SetPrice(BasePremiumRules.CalculateBasePriceFor(cover, calculation));
    }

    private void ApplyDiscounts(Calculation calculation)
    {
        DiscountMarkupRules.Apply(calculation);
    }

    private void UpdateTotals(Calculation calculation)
    {
        calculation.UpdateTotal();
    }

    public static Tariff CreateTariff(string code)
    {
        return new Tariff(code);
    }

    public void AddBasePremiumCalculationRules(IEnumerable<BasePremiumCalculationRule> basePremiumCalculationRules)
    {
        foreach (var b in basePremiumCalculationRules)
            BasePremiumRules.AddBasePriceRule(b.CoverCode, b.ApplyIfFormula, b.BasePriceFormula);
    }

    public void AddDiscountMarkupRules(IEnumerable<DiscountMarkupRule> discountMarkupRules)
    {
        foreach (var d in discountMarkupRules)
            DiscountMarkupRules.AddPercentMarkup(d.ApplyIfFormula, d.ParamValue);
    }

    public void ClearBasePremiumCalculationRules(IEnumerable<BasePremiumCalculationRule> basePremiumCalculationRules)
    {
        foreach (var b in basePremiumCalculationRules)
            BasePremiumRules.RemoveBasePriceRule(b);
    }

    public void ClearDiscountMarkupRules(IEnumerable<DiscountMarkupRule> discountMarkupRules)
    {
        foreach (var d in discountMarkupRules)
            DiscountMarkupRules.RemovePercentMarkup(d);
    }
}