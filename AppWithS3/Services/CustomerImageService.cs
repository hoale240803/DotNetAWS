using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Options;
using AppWithS3.Contracts;

namespace AppWithS3.Services
{
    public class CustomerImageService : ICustomerImageService
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucketName = "dotnet-s3-test-99";
        private readonly IOptions<GlobalSettings> _globalSettings;

        public CustomerImageService(IAmazonS3 s3, IOptions<GlobalSettings> globalSettings)
        {
            _s3 = s3;
            _globalSettings = globalSettings;
        }

        public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
        {
            var s3Conf = _globalSettings.Value;

            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"images/{id}",
                ContentType = file.ContentType,
                InputStream = file.OpenReadStream(),
                Metadata =
            {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName),
            }
            };

            return await _s3.PutObjectAsync(putObjectRequest);
        }

        public async Task<GetObjectResponse> GetImageAsync(Guid id)
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = $"images/{id}"
            };

            return await _s3.GetObjectAsync(getObjectRequest);
        }

        public async Task<DeleteObjectResponse> DeleteImageAsync(Guid id)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = $"images/{id}"
            };

            return await _s3.DeleteObjectAsync(deleteObjectRequest);
        }
    }
}
