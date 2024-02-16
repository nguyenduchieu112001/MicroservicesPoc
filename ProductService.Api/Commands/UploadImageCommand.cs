using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductService.Api.Commands
{
    public class UploadImageCommand : IRequest<UploadImageResult>
    {
        public IFormFile ImageFile { get; set; }
    }
}
