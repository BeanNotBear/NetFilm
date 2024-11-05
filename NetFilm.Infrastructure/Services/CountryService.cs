using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.Exceptions;
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

		/// <summary>
		/// Get country by Id
		/// </summary>
		/// <param name="id">Id of country</param>
		/// <returns>Country Dto</returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<CountryDto> GetById(Guid id)
		{
			var countryDomain = await _countryRepository.GetByIdAsync(id);
			if (countryDomain == null)
			{
				throw new NotFoundException($"Can not found country with Id {id}");
			}
			var countryDtos = _mapper.Map<CountryDto>(countryDomain);
			return countryDtos;
		}
	}
}
