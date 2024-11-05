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
    }
}
