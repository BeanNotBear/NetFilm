using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.BucketDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;

namespace NetFilm.Infrastructure.Services
{
	public class AWSService : IAWSService
	{
		private readonly IAmazonS3 _amazonS3;

		public AWSService(IAmazonS3 amazonS3)
		{
			_amazonS3 = amazonS3;
		}

		public async Task<string> CreateBucketAsync(string bucketName)
		{
			var bucketExistes = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (bucketExistes)
			{
				throw new ExistedEntityException($"Bucket name {bucketName} is already existed!");
			}
			await _amazonS3.PutBucketAsync(bucketName);
			return bucketName;
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

		public async Task<bool> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
		{
			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (!bucketExists)
			{
				throw new NotFoundException($"Can not found bucket with name {bucketName}");
			}
			var request = new PutObjectRequest()
			{
				BucketName = bucketName,
				Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
				InputStream = file.OpenReadStream()
			};
			request.Metadata.Add("Content-Type", file.ContentType);
			await _amazonS3.PutObjectAsync(request);
			return true;
		}

		public async Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync(string bucketName, string? prefix)
		{
			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (!bucketExists)
			{
				throw new NotFoundException($"Can not found bucket with name {bucketName}");
			}
			var request = new ListObjectsV2Request()
			{
				BucketName = bucketName,
				Prefix = prefix
			};
			var result = await _amazonS3.ListObjectsV2Async(request);
			var s3Objects = result.S3Objects.Select(s =>
			{
				var urlRequest = new GetPreSignedUrlRequest()
				{
					BucketName = bucketName,
					Key = s.Key,
					Expires = DateTime.UtcNow.AddMinutes(1)
				};
				return new S3ObjectDto()
				{
					Name = s.Key.ToString(),
					PresignedUrl = _amazonS3.GetPreSignedURL(urlRequest),
				};
			});
			return s3Objects;
		}

		public async Task<S3ObjectDto> GetFileByKeyAsync(string bucketName, string key)
		{
			try
			{
				if (string.IsNullOrEmpty(bucketName))
					throw new ArgumentNullException(nameof(bucketName), "Bucket name cannot be null or empty");

				if (string.IsNullOrEmpty(key))
					throw new ArgumentNullException(nameof(key), "File key cannot be null or empty");

				// Check if bucket exists
				var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
				if (!bucketExists)
					throw new NotFoundException($"Bucket '{bucketName}' not found");
				// Attempt to get the object
				//var s3Object = await _amazonS3.GetObjectAsync(bucketName, key);
				var urlRequest = new GetPreSignedUrlRequest()
				{
					BucketName = bucketName,
					Key = key,
					Expires = DateTime.UtcNow.AddMinutes(1)
				};
				return new S3ObjectDto()
				{
					Name = key,
					PresignedUrl = _amazonS3.GetPreSignedURL(urlRequest),
				};
			}
			catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				throw new NotFoundException($"File with key '{key}' not found in bucket '{bucketName}'");
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving file from S3: {ex.Message}", ex);
			}
		}

		public async Task<bool> DeleteFileAsync(string bucketName, string key)
		{
			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (!bucketExists)
			{
				throw new NotFoundException($"Can not found bucket with name {bucketName}");
			}

			await _amazonS3.DeleteObjectAsync(bucketName, key);
			return true;
		}
	}
}
