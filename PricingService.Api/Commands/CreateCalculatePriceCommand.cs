using MediatR;
using PricingService.Api.Commands.Dtos;
using System.Collections.Generic;

namespace PricingService.Api.Commands;

public class CreateCalculatePriceCommand : IRequest<CreateCalculatePriceResult>
{
    public string Code { get; set; }
    public IList<BasePremiumCalculationRuleDto> BasePremiumCalculationRules { get; set; }
    public IList<DiscountMarkupRuleDto> DiscountMarkupRules { get; set; }
}
