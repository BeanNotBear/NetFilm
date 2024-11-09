using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Domain.Common;
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
		Task<IEnumerable<MovieDto>> GetAllAsync();
		Task<MovieDto> GetByIdAsync(Guid id);
		Task<PagedResult<MovieDto>> GetPaging(MovieQueryParam queryParam);
		Task<MovieDto> UpdateMovieAsync(Guid id, UpdateMovieRequestDto updateMovieRequestDto);
	}
}
