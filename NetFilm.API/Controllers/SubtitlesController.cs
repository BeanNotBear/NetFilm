using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Infrastructure.Services;

namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubtitlesController : ControllerBase
	{
		private readonly IAWSService awsService;
		private readonly ISubtitleService subtitleService;
		private const string BUCKET_SUBTITLE = "netfilm-dotnet-s3-subtitle";

		public SubtitlesController(IAWSService awsService, ISubtitleService subtitleService)
		{
			this.awsService = awsService;
			this.subtitleService = subtitleService;
		}

		[HttpPost]
		[Route("Upload")]
		[ValidateModel]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> Upload([FromForm] AddSubtitleRequestDto addSubtitleRequestDto, string? prefix)
		{
			var randomFileName = Guid.NewGuid().ToString();
			var url = await awsService.UploadVttAsync(addSubtitleRequestDto.File, BUCKET_SUBTITLE, prefix, randomFileName);
			var subtitle = await subtitleService.AddSubtitle(addSubtitleRequestDto.SubtitleName, url.CreateUrl(), addSubtitleRequestDto.MovieId);
			return Ok(subtitle);
		}

		[HttpDelete]
		[Route("{id:guid}")]
		[ValidateModel]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> Update([FromRoute] Guid id, string? prefix)
		{
			var subtitle = await subtitleService.GetSubtitleById(id);
			var url = subtitle.SubtitleUrl;
			var uri = new Uri(url);
			var objectKey = uri.AbsolutePath.TrimStart('/');
			var randomFileName = Guid.NewGuid().ToString();
			await awsService.DeleteFileAsync(BUCKET_SUBTITLE, objectKey);
			await subtitleService.DeleteSubtitle(id);
			return NoContent();
		}
	}
}
