using System.ComponentModel.DataAnnotations;

namespace NetFilm.Application.DTOs.CountryDTOs
{
	public class AddCountryRequestDto
	{
		[Required]
        public string Name { get; set; }
    }
}
