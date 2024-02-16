using MediatR;

namespace PricingService.Api.Commands
{
    public class DeleteCalculatePriceCommand : IRequest<DeleteCalculatePriceResult>
    {
        public string Code { get; set; }
    }
}
