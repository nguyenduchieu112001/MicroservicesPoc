using MediatR;
using PolicySearchService.Api.Queries.Dtos;

namespace PolicySearchService.Api.Commands
{
    public class CreatePolicySearchCommand : IRequest<CreatePolicySearchResult>
    {
        public PolicyDto PolicyDto { get; set; }
    }
}
