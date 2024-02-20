using MediatR;
using PolicySearchService.Domain;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace PolicySearchService.Listeners;

public class PolicyDeletedHandler : INotificationHandler<PolicyTerminated>
{
    private readonly IPolicyRepository policis;

    public PolicyDeletedHandler(IPolicyRepository policis)
    {
        this.policis = policis;
    }

    public async Task Handle(PolicyTerminated notification, CancellationToken cancellationToken)
    {
        var policies = await policis.Find(notification.PolicyNumber);
        foreach (var policy in policies)
            await policis.Delete(policy);
    }
}
