using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.SubtitleDTOs;
using NetFilm.Application.Interfaces;
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
		public async Task<IActionResult> Upload([FromForm] AddSubtitleRequestDto addSubtitleRequestDto, string? prefix)
		{
			if (addSubtitleRequestDto.Files.Count != addSubtitleRequestDto.SubtitleName.Count)
			{
				return BadRequest();
			}
			List<SubtitleDto> subtitles = new List<SubtitleDto>();
			var numberOfItems = addSubtitleRequestDto.Files.Count;
			for (var i = 0; i < numberOfItems; i++)
			{
				var url = await awsService.UploadSrtAsync(addSubtitleRequestDto.Files[i], BUCKET_SUBTITLE, prefix);
				subtitles.Add(await subtitleService.AddSubtitle(addSubtitleRequestDto.SubtitleName[i], url, addSubtitleRequestDto.MovieId));
			}
			return Ok(subtitles);
		}
	}
}
