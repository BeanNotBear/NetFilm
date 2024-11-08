﻿using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
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
		/// Add country to Database
		/// </summary>
		/// <param name="addCountryRequestDto">Add country request</param>
		/// <returns>created country</returns>
		public async Task<CountryDto> Add(AddCountryRequestDto addCountryRequestDto)
		{
			var isExisted = await _countryRepository.ExistsByNameAsync(addCountryRequestDto.Name);
			if (isExisted)
			{
				throw new ExistedEntityException($"{addCountryRequestDto.Name} is already existed!");
			}
			var countryDomain = _mapper.Map<Country>(addCountryRequestDto);

			var createdCountryDomain = await _countryRepository.AddAsync(countryDomain);
			var createdCountryDto = _mapper.Map<CountryDto>(createdCountryDomain);
			return createdCountryDto;
		}

		/// <summary>
		/// Hard delete country
		/// </summary>
		/// <param name="id">country id</param>
		/// <returns>boolean</returns>
		/// <exception cref="NotFoundException">Throw when not found</exception>
		/// <exception cref="Exception">Throw when can not delete</exception>
		public async Task<bool> HardDelete(Guid id)
		{
			var isExisted = await _countryRepository.ExistsAsync(id);
			if (!isExisted)
			{
				throw new NotFoundException($"Can not found country with Id: {id}");
			}
			var isDeleted = await _countryRepository.DeleteAsync(id);
			if (!isDeleted)
			{
				throw new Exception("Some things went wrong!");
			}
			return true;
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

		/// <summary>
		/// Soft delete country
		/// </summary>
		/// <param name="id"> country id</param>
		/// <returns>country dto</returns>

		public async Task<CountryDto> SoftDelete(Guid id)
		{
			var isExisted = await _countryRepository.ExistsAsync(id);
			if (!isExisted)
			{
				throw new NotFoundException($"Can not found country with Id: {id}");
			}
			var countryDomain = await _countryRepository.SoftDeleteAsync(id);
			var countryDto = _mapper.Map<CountryDto>(countryDomain);
			return countryDto;
		}

		/// <summary>
		/// Update country
		/// </summary>
		/// <param name="updateCountryRequestDto"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<CountryDto> Update(Guid id, UpdateCountryRequestDto updateCountryRequestDto)
		{
			var countryDomain = await _countryRepository.GetByIdAsync(id);
			if (countryDomain == null)
			{
				throw new NotFoundException($"Can not found country with Id: {id}");
			}
			countryDomain.Name = updateCountryRequestDto.Name;
			var updatedCountryDomain = await _countryRepository.UpdateAsync(countryDomain);
			if (updatedCountryDomain == null)
			{
				throw new Exception("Some things went wrong!");
			}
			var updatedCountryDto = _mapper.Map<CountryDto>(updatedCountryDomain);
			return updatedCountryDto;
		}
	}
}
