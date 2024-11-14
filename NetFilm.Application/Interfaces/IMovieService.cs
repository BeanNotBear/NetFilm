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
		Task<MovieDetailDto> AddMovieAsync(string movieName, string movieUrl);
		Task<MovieDetailDto> UpdateMovieAsync(Guid id, AddMovieRequestDto addMovieRequestDto);
		Task<MovieDetailDto> UpdateThumbnailAsync(Guid id, string thumbnail);
		Task<IEnumerable<MovieDto>> GetAllAsync();
		Task<MovieDetailDto> GetByIdAsync(Guid id);
		Task<PagedResult<MovieDto>> GetPaging(MovieQueryParam queryParam);
		Task<PagedResult<MovieDto>> GetMoviePaging(MovieQueryParam queryParam);
		Task<MovieDto> UpdateMovieAsync(Guid id, UpdateMovieRequestDto updateMovieRequestDto);
		Task<MovieDto> SoftDeleteAsync(Guid id);
	}
}
