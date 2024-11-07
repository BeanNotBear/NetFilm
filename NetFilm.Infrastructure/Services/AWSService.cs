using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.BucketDTOs;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using System.Net;

namespace NetFilm.Infrastructure.Services
{
	public class AWSService : IAWSService
	{
		private readonly IAmazonS3 _amazonS3;
		private const string DISTRIBUTION_DOMAIN = "https://dqg1h1bamqrgk.cloudfront.net";
		private const int CHUNK_DURATION_SECONDS = 15;

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

		public async Task<string> UploadImageAsync(IFormFile file, string bucketName, string? prefix)
		{
			var fileExtension = Path.GetExtension(file.FileName);
			List<string> imageExtensions = new List<string>
			{
				".jpg",
				".jpeg",
				".png",
				".gif",
				".bmp",
				".tiff",
				".tif",
				".webp",
				".svg",
				".ico"
			};
			if (imageExtensions.Contains(fileExtension))
			{
				string key = await UploadFileAsync(file, bucketName, prefix);
				return key;
			}
			throw new FileNotAllowException($"File with {fileExtension} is not allowed!");
		}

		public async Task<string> UploadVideoAsync(IFormFile file, string bucketName, string? prefix)
		{
			var fileExtension = Path.GetExtension(file.FileName);
			List<string> videoExtensions = new List<string>
			{
				".mp4",
				".webm",
				".ogg",
				".mov",
				".avi",
				".flv",
				".3gp",
				".mkv"
			};
			if (videoExtensions.Contains(fileExtension))
			{
				string key = await UploadFileAsync(file, bucketName, prefix);
				return key;
			}
			throw new FileNotAllowException($"File with {fileExtension} is not allowed!");
		}

		public async Task<string> UploadSrtAsync(IFormFile file, string bucketName, string? prefix)
		{
			var fileExtension = Path.GetExtension(file.FileName);
			if (fileExtension == ".srt")
			{
				string key = await UploadFileAsync(file, bucketName, prefix);
				return key;
			}
			throw new FileNotAllowException($"File with {fileExtension} is not allowed!");
		}

		private async Task<string> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
		{
			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (!bucketExists)
			{
				throw new NotFoundException($"Cannot find bucket with name {bucketName}");
			}

			var fileExtension = Path.GetExtension(file.FileName);

			var key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix}/{file.FileName}";

			var request = new PutObjectRequest
			{
				BucketName = bucketName,
				Key = key,
				InputStream = file.OpenReadStream()
			};

			request.Metadata.Add("Content-Type", file.ContentType);

			await _amazonS3.PutObjectAsync(request);

			return key;
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
					Expires = DateTime.UtcNow.AddHours(12)
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

				var s3Object = await _amazonS3.GetObjectAsync(bucketName, key);

				return new S3ObjectDto
				{
					Name = s3Object.Key,
					PresignedUrl = GetUrlCloudFront(s3Object.Key),
				};
			}
			catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				throw new NotFoundException($"File with key '{key}' not found in bucket '{bucketName}'");
			}
			catch (AmazonS3Exception ex) when (ex.ErrorCode == "InvalidAccessKeyId")
			{
				throw new Exception("Invalid AWS credentials. Please check your access key.", ex);
			}
			catch (AmazonS3Exception ex) when (ex.ErrorCode == "SignatureDoesNotMatch")
			{
				throw new Exception("Invalid AWS credentials. Please check your secret key.", ex);
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

		private string GetUrlCloudFront(string key)
		{
			return $"{DISTRIBUTION_DOMAIN}/{key}";
		}
	}
}
