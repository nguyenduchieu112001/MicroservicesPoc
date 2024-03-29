﻿using MediatR;
using PolicySearchService.Domain;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace PolicySearchService.Listeners;

public class PolicyTerminatedHandler : INotificationHandler<PolicyTerminated>
{
    private readonly IPolicyRepository policis;

    public PolicyTerminatedHandler(IPolicyRepository policis)
    {
        this.policis = policis;
    }

    public async Task Handle(PolicyTerminated notification, CancellationToken cancellationToken)
    {
        await policis.Delete(notification.PolicyNumber);
    }
}
