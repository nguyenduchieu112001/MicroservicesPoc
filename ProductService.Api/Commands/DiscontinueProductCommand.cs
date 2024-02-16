using MediatR;

namespace ProductService.Api.Commands;

public class DiscontinueProductCommand : IRequest<DiscontinueProductResult>
{
    public long ProductId { get; set; }
}