using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Interfaces;

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
	}
}
