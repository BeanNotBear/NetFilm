using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.BucketDTOs;
using NetFilm.Infrastructure.Attributes;

namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BucketsController : ControllerBase
	{
		[HttpPost]
		[ValidateModel]
		public Task<IActionResult> Add([FromBody] AddBucketRequestDto addBucketRequestDto)
		{

		}
	}
}
