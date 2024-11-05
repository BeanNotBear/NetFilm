using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Infrastructure.Attributes;

namespace NetFilm.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		private readonly ICountryService countryService;

		public CountriesController(ICountryService countryService)
		{
			this.countryService = countryService;
		}

		/// <summary>
		/// Get all countries
		/// </summary>
		/// <returns>list of countries</returns>
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var countries = await countryService.GetAll();
			return Ok(countries);
		}


		/// <summary>
		/// Get country by id 
		/// </summary>
		/// <param name="id"></param>
		/// <returns>country</returns>
		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var country = await countryService.GetById(id);
			return Ok(country);
		}

		/// <summary>
		/// Add a new country
		/// </summary>
		/// <param name="addCountryRequestDto">add country request</param>
		/// <returns>country</returns>
		[HttpPost]
		[ValidateModel]
		public async Task<IActionResult> Add([FromBody] AddCountryRequestDto addCountryRequestDto)
		{
			var country = await countryService.Add(addCountryRequestDto);
			return CreatedAtAction(nameof(GetById), new { id = country.Id }, country);
		}

		/// <summary>
		/// Hard delete a country
		/// </summary>
		/// <param name="id">id of country</param>
		/// <returns>no content</returns>
		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> HardDelete([FromRoute] Guid id)
		{
			await countryService.HardDelete(id);
			return NoContent();
		}

		/// <summary>
		/// Soft delete a country
		/// </summary>
		/// <param name="id">id of country</param>
		/// <returns>country</returns>
		[HttpPatch]
		[Route("{id:guid}")]
		public async Task<IActionResult> SoftDelete([FromRoute] Guid id)
		{
			var country = await countryService.SoftDelete(id);
			return Ok(country);
		}

		/// <summary>
		/// Update a country
		/// </summary>
		/// <param name="id">id of country</param>
		/// <param name="updateCountryRequestDto">new data need update</param>
		/// <returns>country</returns>
		[HttpPut]
		[Route("{id:guid}")]
		[ValidateModel]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCountryRequestDto updateCountryRequestDto)
		{
			var country = await countryService.Update(id, updateCountryRequestDto);
			return Ok(country);
		}
	}
}
