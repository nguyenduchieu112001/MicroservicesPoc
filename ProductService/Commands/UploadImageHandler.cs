using MediatR;
using ProductService.Api.Commands;

namespace ProductService.Commands
{
    public class UploadImageHandler : IRequestHandler<UploadImageCommand, UploadImageResult>
    {
        private readonly IWebHostEnvironment env;
        public UploadImageHandler(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public async Task<UploadImageResult> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contentPath = env.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Check the allowed extenstions
                var ext = Path.GetExtension(request.ImageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    throw new AggregateException(msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                using var stream = new FileStream(fileWithPath, FileMode.Create);
                await request.ImageFile.CopyToAsync(stream, cancellationToken);
                return new UploadImageResult { ImageFileName = $"/{newFileName}" };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error has occurred", ex);
            }
        }
    }
}
