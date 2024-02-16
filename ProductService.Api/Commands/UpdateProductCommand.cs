using MediatR;
using ProductService.Api.Commands.Dtos;

namespace ProductService.Api.Commands
{
    public class UpdateProductCommand : IRequest<UpdateProductResult>
    {
        public ProductDraftDto ProductDraft { get; set; }
    }
}
