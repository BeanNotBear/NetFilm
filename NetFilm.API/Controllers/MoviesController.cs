using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly IAWSService awsService;
		private readonly IMovieService movieService;
		private const string BUCKET_MOVIE = "netfilm-dotnet-s3";
		private const string BUCKET_IMAGE = "netfilm-dotnet-s3-image";
		private const string BUCKET_SUBTITLE = "netfilm-dotnet-s3-subtitle";

		public MoviesController(IAWSService awsService, IMovieService movieService)
		{
			this.awsService = awsService;
			this.movieService = movieService;
		}

		[HttpPost]
		[Route("Upload")]
		public async Task<IActionResult> UploadVideoAsync(IFormFile file, string? prefix)
		{
			string movieUrl = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix}/{file.FileName}";
			await awsService.UploadVideoAsync(file, BUCKET_MOVIE, prefix);
			var movie = await movieService.AddMovieAsync(file.FileName, movieUrl);
			return Ok(movie);
		}

		[HttpPut]
		[Route("{id:guid}/Add/Details")]
		[ValidateModel]
		public async Task<IActionResult> AddMovieDetails([FromRoute] Guid id, [FromBody] AddMovieRequestDto addMovieRequestDto)
		{
			var movie = await movieService.UpdateMovieAsync(id, addMovieRequestDto);
			return Ok(movie);
		}

		[HttpPatch]
		[Route("{id:guid}/Upload/Thumbnail")]
		public async Task<IActionResult> UploadThumbnail([FromRoute] Guid id, string? prefix, IFormFile file)
		{
			string movieUrl = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix}/{file.FileName}";
			var movie = await movieService.UpdateThumbnailAsync(id, movieUrl);
			string thumbnail = await awsService.UploadImageAsync(file, BUCKET_IMAGE, prefix);
			return Ok(movie);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFile(string bucketName, string? prefix)
		{
			var files = await awsService.GetAllFilesAsync(bucketName, prefix);
			return Ok(files);
		}

		[HttpGet]
		[Route("watch")]
		public async Task<IActionResult> GetMovieById(string bucketName, string key)
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
