using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
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
		//[Authorize(AuthenticationSchemes = "Bearer")]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> UploadVideoAsync(IFormFile file, string? prefix)
		{
			string randomFileName = Guid.NewGuid().ToString();
			var url = await awsService.UploadVideoAsync(file, BUCKET_MOVIE, prefix, randomFileName);
			var movie = await movieService.AddMovieAsync(file.FileName, url);
			return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
		}

		[HttpPut]
		[Route("{id:guid}/Add/Details")]
		[ValidateModel]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> AddMovieDetails([FromRoute] Guid id, [FromBody] AddMovieRequestDto addMovieRequestDto, string? prefix)
		{
			var movie = await movieService.UpdateMovieAsync(id, addMovieRequestDto);
			return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
		}

		[HttpPatch]
		[Route("{id:guid}/view")]
		public async Task<IActionResult> AddView([FromRoute] Guid id)
		{
			var movie = await movieService.AddView(id);
			return Ok(movie);
		}

		[HttpPost]
		[Route("{id:guid}/Add/Poster")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> UpdatePoster([FromRoute] Guid id, IFormFile file, string? prefix)
		{
			string randomFileName = Guid.NewGuid().ToString();
			string thumbnail = await awsService.UploadImageAsync(file, BUCKET_IMAGE, prefix, randomFileName);
			var movieThumbnail = await movieService.UpdateThumbnailAsync(id, thumbnail.CreateUrl());
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
		[Route("admin/spec")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> GetPaging([FromQuery] MovieQueryParam movieQueryParam)
		{
			var movie = await movieService.GetPaging(movieQueryParam);
			return Ok(movie);
		}

		[HttpGet]
		[Route("spec")]
		public async Task<IActionResult> GetPagingViewer([FromQuery] MovieQueryParam movieQueryParam)
		{
			var movie = await movieService.GetMoviePaging(movieQueryParam);
			return Ok(movie);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteFile(string bucketName, string key)
		{
			var isDeleted = await awsService.DeleteFileAsync(bucketName, key);
			return NoContent();
		}

		[HttpPatch]
		[Route("{id:guid}")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
		{
			var movie = await movieService.SoftDeleteAsync(id);
			return Ok(movie);
		}

		// update movie video
		[HttpPatch]
		[Route("{id:guid}/upload/video")]
		public async Task<IActionResult> UpdateVideo([FromRoute] Guid id, IFormFile file, int duration, string? prefix)
		{
			string randomFileName = Guid.NewGuid().ToString();
			if (file != null)
			{
				var movie = await movieService.GetByIdAsync(id);
				movie.Duration = duration;
				var url = movie.Movie_Url;
				var uri = new Uri(url);
				var objectKey = uri.AbsolutePath.TrimStart('/');
				await awsService.DeleteFileAsync(BUCKET_MOVIE, objectKey);
				var movieUrl = await awsService.UploadVideoAsync(file, BUCKET_MOVIE, prefix, randomFileName);
				movie.Movie_Url = movieUrl.CreateUrl();
				await movieService.UpdateMovieDetails(movie);
			}
			return Ok(file);
		}

		// update movie information
		[HttpPatch]
		[Route("{id:guid}/update/information")]
		public async Task<IActionResult> UpdateInformation([FromRoute] Guid id, [FromBody] UpdateMovieRequestDto updateMovieRequestDto)
		{

			var movie = await movieService.UpdateMovieInformation(id, updateMovieRequestDto);
			return Ok(movie);
		}

		[HttpPatch]
		[Route("{id:guid}/update/poster")]
		//[Authorize(AuthenticationSchemes = "Bearer")]
		//[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> UpdateNewPoster([FromRoute] Guid id, IFormFile file, string? prefix)
		{

			if (file != null)
			{
				var randomFileName = Guid.NewGuid().ToString();
				var movie = await movieService.GetByIdAsync(id);
				var url = movie.Thumbnail;
				if(!string.IsNullOrWhiteSpace(url))
				{
					var uri = new Uri(url);
					var objectKey = uri.AbsolutePath.TrimStart('/');
					await awsService.DeleteFileAsync(BUCKET_IMAGE, objectKey);
				}
				var poster = await awsService.UploadImageAsync(file, BUCKET_IMAGE, prefix, randomFileName);
				var updatedMovie = await movieService.UpdateThumbnailAsync(id, poster.CreateUrl());
				return Ok(updatedMovie);
			}
			return BadRequest();
		}
	}
}
