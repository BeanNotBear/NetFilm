using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Interfaces;

namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly IAWSService awsService;

		public MoviesController(IAWSService awsService)
		{
			this.awsService = awsService;
		}

		[HttpPost]
		[Route("Upload")]
		public async Task<IActionResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
		{
			var isUploaded = await awsService.UploadFileAsync(file, bucketName, prefix);
			return Ok(isUploaded);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFile(string bucketName, string? prefix)
		{
			var files = await awsService.GetAllFilesAsync(bucketName, prefix);
			return Ok(files);
		}

		[HttpGet]
		[Route("getByKey")]
		[ResponseCache(Duration = 3600)]
		public async Task<IActionResult> GetByKey(string bucketName, string key)
		{
			var file = await awsService.GetFileByKeyAsync(bucketName, key);
			return Ok(file);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteFile(string bucketName, string key)
		{
			var isDeleted = await awsService.DeleteFileAsync(bucketName, key);
			return NoContent();
		}
	}
}
