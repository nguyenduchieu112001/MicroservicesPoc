using MediatR;
using PricingService.Api.Commands;
using PricingService.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PricingService.Commands
{
    public class DeleteCalculatePriceHandler : IRequestHandler<DeleteCalculatePriceCommand, DeleteCalculatePriceResult>
    {
        private readonly IDataStore dataStore;
        public DeleteCalculatePriceHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        public async Task<DeleteCalculatePriceResult> Handle(DeleteCalculatePriceCommand request, CancellationToken cancellationToken)
        {
            var existingTariff = await dataStore.Tariffs[request.Code];
            if (existingTariff == null)
            {
                return new DeleteCalculatePriceResult();
            }

            await dataStore.Tariffs.Delete(existingTariff);
            await dataStore.CommitChanges();
            return new DeleteCalculatePriceResult
            {
                PriceId = new Guid("11111111-1111-1111-1111-111111111111"),
            };
        }
    }
}
