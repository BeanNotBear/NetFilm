using NetFilm.Application.DTOs.CountryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
	public interface ICountryService
	{
		Task<IEnumerable<CountryDto>> GetAll();
		Task<CountryDto> GetById(Guid id);
		Task<CountryDto> Add(AddCountryRequestDto addCountryRequestDto);
		Task<bool> HardDelete(Guid id);
		Task<CountryDto> SoftDelete(Guid id);
		Task<CountryDto> Update(Guid id, UpdateCountryRequestDto updateCountryRequestDto);
	}
}
