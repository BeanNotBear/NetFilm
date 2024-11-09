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
		private const string DISTRIBUTION_DOMAIN = "https://dqg1h1bamqrgk.cloudfront.net";

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
			var movie = await movieService.AddMovieAsync(file.FileName, movieUrl);
			await awsService.UploadVideoAsync(file, BUCKET_MOVIE, prefix);
			return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
		}

		[HttpPut]
		[Route("{id:guid}/Add/Details")]
		[ValidateModel]
		public async Task<IActionResult> AddMovieDetails([FromRoute] Guid id, [FromForm] AddMovieRequestDto addMovieRequestDto, string? prefix)
		{
			var movie = await movieService.UpdateMovieAsync(id, addMovieRequestDto);
			string fileName = addMovieRequestDto.File.FileName;
			string movieUrl = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix}/{fileName}";
			var movieThumbnail = await movieService.UpdateThumbnailAsync(id, movieUrl);
			string thumbnail = await awsService.UploadImageAsync(addMovieRequestDto.File, BUCKET_IMAGE, prefix);
			return CreatedAtAction(nameof(GetById), new { id = movieThumbnail.Id }, movieThumbnail);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFile()
		{
			var movies = await movieService.GetAllAsync();
			return Ok(movies);
		}

		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var movie = await movieService.GetByIdAsync(id);
			return Ok(movie);
		}

		[HttpGet]
		[Route("watch")]
		public async Task<IActionResult> GetMovieById(string bucketName, string key)
		{
			var file = await awsService.GetFileByKeyAsync(bucketName, key);
			return Ok(file);
		}

		[HttpGet]
		[Route("spec")]
		public async Task<IActionResult> GetPaging([FromQuery] MovieQueryParam movieQueryParam)
		{
			var movie = await movieService.GetPaging(movieQueryParam);
			return Ok(movie);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteFile(string bucketName, string key)
		{
			var isDeleted = await awsService.DeleteFileAsync(bucketName, key);
			return NoContent();
		}
	}
}
