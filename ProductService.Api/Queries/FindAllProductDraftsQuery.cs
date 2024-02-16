using MediatR;
using ProductService.Api.Queries.Dtos;
using System.Collections.Generic;

namespace ProductService.Api.Queries;

public class FindAllProductDraftsQuery : IRequest<IEnumerable<ProductDto>>
{
}
