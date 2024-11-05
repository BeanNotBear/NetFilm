using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.DTOs.CountryDTOs
{
	public class UpdateCountryRequestDto
	{
		[Required]
		public string Name { get; set; }
	}
}
