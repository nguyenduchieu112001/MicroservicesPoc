using MediatR;
using PaymentService.Domain;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Listeners;

public class PolicyTerminatedHandler : INotificationHandler<PolicyTerminated>
{
    private readonly IDataStore dataStore;

    public PolicyTerminatedHandler(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task Handle(PolicyTerminated notification, CancellationToken cancellationToken)
    {

        using (dataStore)
        {
            var policyAccount = await dataStore.PolicyAccounts.FindByNumber(notification.PolicyNumber);

            if (policyAccount == null) return;

            policyAccount.Close(notification.PolicyTo, notification.AmountToReturn);

            dataStore.PolicyAccounts.Update(policyAccount);

            await dataStore.CommitChanges();
        }


    }
}