using Microsoft.AspNetCore.Http;

namespace AuthService.Domain.Dtos
{
    public class UploadAvatarRequest
    {
        public IFormFile file { get; set; }
    }
}
