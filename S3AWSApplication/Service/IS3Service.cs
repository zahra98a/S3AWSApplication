using S3AWSApplication.Models;
using System.Threading.Tasks;

namespace S3AWSApplication.Service
{
    public interface IS3Service
    {
        Task <S3Response> CreateBucketAsync(string bucketName);
        Task<S3Response> DeleteBucketAsync(string bucketName);
        Task UploadFileAsync(string bucketName);
        Task DeleteObjectAsync(string bucketName);
    }
}
