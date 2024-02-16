using MediatR;
using PricingService.Api.Queries;
using PricingService.Api.Queries.Dtos;
using PricingService.Domain;
using PricingService.Queries.Mappers;
using System.Threading;
using System.Threading.Tasks;

namespace PricingService.Queries
{
    public class FindCalculateByCodeHandler : IRequestHandler<FindCalculateByCodeQuery, TariffDto>
    {
        private readonly IDataStore dataStore;
        public FindCalculateByCodeHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        public async Task<TariffDto> Handle(FindCalculateByCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await dataStore.Tariffs[request.ProductCode];
            return result != null
            ? new TariffDto
            {
                Code = result.Code,
                BasePremiumCalculationRules = result.BasePremiumRules != null ? TariffMapper.ToBasePremiumDtoList(result.BasePremiumRules) : null,
                DiscountMarkupRules = result.DiscountMarkupRules != null ? TariffMapper.ToDiscountDtoList(result.DiscountMarkupRules) : null
            }
            : null;
        }
    }
}
