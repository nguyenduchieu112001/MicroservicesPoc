using ChatService.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Listeners
{
    public class PolicyTerminateHandler : INotificationHandler<PolicyTerminated>
    {
        private readonly IHubContext<AgentChatHub> chatHubContext;
        public PolicyTerminateHandler(IHubContext<AgentChatHub> chatHubContext)
        {
            this.chatHubContext = chatHubContext;
        }
        public async Task Handle(PolicyTerminated notification, CancellationToken cancellationToken)
        {
            await chatHubContext.Clients.All.SendAsync("ReceiveNotification",
            $"{notification.ProductCode} just return policy for {notification.AmountToReturn}!!!");
        }
    }
}
