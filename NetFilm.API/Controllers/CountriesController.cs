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

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var countries = await countryService.GetAll();
			return Ok(countries);
		}
	}
}
