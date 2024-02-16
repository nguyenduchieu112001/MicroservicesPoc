namespace PricingService.Api.Queries.Dtos
{
    public class BasePremiumCalculationRuleDto
    {
        public string CoverCode { get; set; }
        public string ApplyIfFormula { get; set; }
        public string BasePriceFormula { get; set; }
    }
}
