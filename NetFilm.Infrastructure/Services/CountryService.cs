using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Interfaces;

namespace NetFilm.Infrastructure.Services
{
	/// <summary>
	/// CountryService help communicate between repository and controller.
	/// </summary>
	public class CountryService : ICountryService
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMapper _mapper;

		public CountryService(ICountryRepository countryRepository, IMapper mapper)
		{
			_countryRepository = countryRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get all countrys
		/// </summary>
		/// <returns>List of countryDtos</returns>
		public async Task<IEnumerable<CountryDto>> GetAll()
		{
			var countryDomains = await _countryRepository.GetAllAsync();
			var countryDtos = _mapper.Map<IEnumerable<CountryDto>>(countryDomains);
			return countryDtos;
		}
	}
}
