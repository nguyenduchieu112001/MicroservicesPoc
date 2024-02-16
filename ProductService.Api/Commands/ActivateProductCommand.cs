using MediatR;

namespace ProductService.Api.Commands;

public class ActivateProductCommand : IRequest<ActivateProductResult>
{
    public long ProductId { get; set; }
}