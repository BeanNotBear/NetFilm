using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.BucketDTOs;
using NetFilm.Application.Interfaces;


namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BucketsController : ControllerBase
	{

		private readonly IAWSService awsService;

		public BucketsController(IAWSService awsService)
		{
			this.awsService = awsService;
		}

		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> Add([FromBody] AddBucketRequestDto addBucketRequestDto)
		{
			var bucketName = await awsService.CreateBucketAsync(addBucketRequestDto.BucketName);
			return Created("buckets", $"Bucket {bucketName} created");
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var listOfBucket = await awsService.GetAllBucketAsync();
			return Ok(listOfBucket);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string bucketName)
		{
			var isDeleted = await awsService.DeleteBucketAsync(bucketName);
			return NoContent();
		}
	}
}
