using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.BucketDTOs;

namespace NetFilm.Application.Interfaces
{
	public interface IAWSService
	{
		Task<string> CreateBucketAsync(string bucketName);
		Task<IEnumerable<string>> GetAllBucketAsync();
		Task<bool> DeleteBucketAsync(string bucketName);
		Task<bool> UploadFileAsync(IFormFile file, string bucketName, string? prefix);
		Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync(string bucketName, string? prefix);
		Task<S3ObjectDto> GetFileByKeyAsync(string bucketName, string key);
		Task<bool> DeleteFileAsync(string bucketName, string key);
	}
}
