using NetFilm.Application.DTOs.MovieDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
	public interface IMovieService
	{
		Task<MovieDto> AddMovieAsync(string movieName, string movieUrl);
		Task<MovieDto> UpdateMovieAsync(Guid id, AddMovieRequestDto addMovieRequestDto);
		Task<MovieDto> UpdateThumbnailAsync(Guid id, string thumbnail);
	}
}
