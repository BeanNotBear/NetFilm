using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.AdvertiseDTOs;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Infrastructure.Services;

namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdvertiseController : ControllerBase
	{
		private readonly IAWSService _awsService;
		private readonly IAdvertiseService _advertiseService;
		private const string BUCKET_IMAGE = "netfilm-dotnet-s3-image";

		public AdvertiseController(IAWSService awsService, IAdvertiseService advertiseService)
		{
			_advertiseService = advertiseService;
			_awsService = awsService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAdvertise()
		{
			var advertises = await _advertiseService.GetAllAdvertises();
			return Ok(advertises);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetAdvertiseById(Guid id)
		{
			var advertise = await _advertiseService.GetAdvertiseById(id);
			return Ok(advertise);
		}

		[HttpPost]
		public async Task<IActionResult> AddAdvertise([FromForm] AdvertiseFileDto imageFile, string? prefix, [FromForm] AddAdvertiseDto addAdvertiseDto)
		{
			string randomFileName = Guid.NewGuid().ToString();
			var url = await _awsService.UploadImageAsync(imageFile.File, BUCKET_IMAGE, prefix, randomFileName);
			var advertise = await _advertiseService.AddAdvertise(addAdvertiseDto, url.CreateUrl());
			return CreatedAtAction(nameof(GetAdvertiseById), new { id = advertise.Id }, advertise);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAdvertise(Guid id, IFormFile? imageFile, string? prefix, [FromForm] UpdateAdvertiseDto updateAdvertiseDto)
		{
			string randomFileName = Guid.NewGuid().ToString();
			string imageUrl = "";
			if (imageFile != null)
			{
				string fileName = imageFile.FileName;
				if (string.IsNullOrEmpty(fileName))
				{
					imageUrl = await _awsService.UploadImageAsync(imageFile, BUCKET_IMAGE, prefix, randomFileName);
				}
			}
			var advertise = await _advertiseService.UpdateAdvertise(id, updateAdvertiseDto, imageUrl.CreateUrl());
			return Ok(advertise);
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvertise(Guid id)
        {
            await _advertiseService.HardDelete(id);
            return NoContent();
        }

        [HttpGet("PageResult")]
        public async Task<IActionResult> GetAdvertisePageResult([FromQuery] AdvertiseQueryParams advertiseQueryParams)
        {
            var categories = await _advertiseService.GetAdvertisePagedResult(advertiseQueryParams);
            return Ok(categories);
        }
    }
}
