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
		public async Task<IActionResult> Upload([FromForm] AddSubtitleRequestDto addSubtitleRequestDto, string? prefix)
		{

			var url = await awsService.UploadSrtAsync(addSubtitleRequestDto.File, BUCKET_SUBTITLE, prefix);
			var subtitle = await subtitleService.AddSubtitle(addSubtitleRequestDto.SubtitleName, url.CreateUrl(), addSubtitleRequestDto.MovieId);
			return Ok(subtitle);
		}
	}
}
