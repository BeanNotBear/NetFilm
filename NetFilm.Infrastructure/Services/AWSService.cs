using Amazon.S3;
using NetFilm.Application.Exceptions;

namespace NetFilm.Infrastructure.Services
{
	public class AWSService
	{
		private readonly IAmazonS3 _amazonS3;

		public AWSService(IAmazonS3 amazonS3)
		{
			_amazonS3 = amazonS3;
		}

		public async Task<bool> CreateBucketAsync(string bucketName)
		{
			var bucketExistes = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (bucketExistes)
			{
				throw new ExistedEntityException($"Bucket name {bucketName} is already existed!");
			}
			await _amazonS3.PutBucketAsync(bucketName);
			return true;
		}

		public async Task<IEnumerable<string>> GetAllBucketAsync()
		{
			var data = await _amazonS3.ListBucketsAsync();
			var buckets = data.Buckets.Select(b => { return b.BucketName; });
			return buckets;
		}

		public async Task<bool> DeleteBucketAsync(string bucketName)
		{
			var bucketExistes = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (!bucketExistes)
			{
				throw new NotFoundException($"Can not found bucket with name {bucketName}");
			}
			await _amazonS3.DeleteBucketAsync(bucketName);
			return true;
		}
	}
}
