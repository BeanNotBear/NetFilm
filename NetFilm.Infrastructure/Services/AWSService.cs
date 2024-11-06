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

		private async Task<string> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
		{
			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
			if (!bucketExists)
			{
				throw new NotFoundException($"Cannot find bucket with name {bucketName}");
			}

			var fileExtension = Path.GetExtension(file.FileName);
			var generatedFileName = $"{Guid.NewGuid()}{fileExtension}";

			var key = string.IsNullOrEmpty(prefix) ? generatedFileName : $"{prefix?.TrimEnd('/')}/{generatedFileName}";

			var request = new PutObjectRequest()
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

				// Calculate expiration time (default to 12 hours, max 7 days)
				var expirationTime = DateTime.UtcNow.AddHours(12);
				var maxExpirationTime = DateTime.UtcNow.AddDays(7);

				if (expirationTime > maxExpirationTime)
					expirationTime = maxExpirationTime;

				var urlRequest = new GetPreSignedUrlRequest
				{
					BucketName = bucketName,
					Key = key,
					Expires = expirationTime,
					Protocol = Protocol.HTTPS,
					// Add response headers to force download or inline display
					ResponseHeaderOverrides = new ResponseHeaderOverrides
					{
						ContentType = "video/mp4", // Adjust content type as needed
												   // ContentDisposition = "attachment; filename=\"" + key + "\"" // Uncomment to force download
					}
				};

				string presignedUrl = _amazonS3.GetPreSignedURL(urlRequest);

				// Validate the URL was generated successfully
				if (string.IsNullOrEmpty(presignedUrl))
					throw new Exception("Failed to generate pre-signed URL");

				return new S3ObjectDto
				{
					Name = key,
					PresignedUrl = presignedUrl,
					ExpirationTime = expirationTime
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

		public async Task<S3ObjectDto> GetVideoByKeyAsync(string bucketName, string key)
		{
			try
			{
				if (string.IsNullOrEmpty(bucketName))
					throw new ArgumentNullException(nameof(bucketName), "Bucket name cannot be null or empty");
				if (string.IsNullOrEmpty(key))
					throw new ArgumentNullException(nameof(key), "File key cannot be null or empty");

				// Kiểm tra nếu bucket có tồn tại
				var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
				if (!bucketExists)
					throw new NotFoundException($"Bucket '{bucketName}' not found");

				// Thiết lập thời gian hết hạn (mặc định là 12 giờ, tối đa là 7 ngày)
				var expirationTime = DateTime.UtcNow.AddHours(12);
				var maxExpirationTime = DateTime.UtcNow.AddDays(7);
				if (expirationTime > maxExpirationTime)
					expirationTime = maxExpirationTime;

				// Tạo presigned URL cho video với đúng content-type
				var urlRequest = new GetPreSignedUrlRequest
				{
					BucketName = bucketName,
					Key = key,
					Expires = expirationTime,
					Protocol = Protocol.HTTPS,
					// Đảm bảo rằng Content-Type là video/mp4 để trình duyệt hiểu là video
					ResponseHeaderOverrides = new ResponseHeaderOverrides
					{
						ContentType = "video/mp4" // Có thể thay đổi nếu định dạng khác như .avi, .mkv, .webm,...
					}
				};

				string presignedUrl = _amazonS3.GetPreSignedURL(urlRequest);

				// Kiểm tra URL đã được tạo thành công chưa
				if (string.IsNullOrEmpty(presignedUrl))
					throw new Exception("Failed to generate pre-signed URL");

				return new S3ObjectDto
				{
					Name = key,
					PresignedUrl = presignedUrl,
					ExpirationTime = expirationTime
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
	}
}
