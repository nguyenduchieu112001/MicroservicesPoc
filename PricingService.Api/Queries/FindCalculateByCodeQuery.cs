using MediatR;
using PricingService.Api.Queries.Dtos;

namespace PricingService.Api.Queries
{
    public class FindCalculateByCodeQuery : IRequest<TariffDto>
    {
        public string ProductCode { get; set; }
    }
}
