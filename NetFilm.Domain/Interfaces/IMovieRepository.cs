using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFilm.Domain.Entities;

namespace NetFilm.Domain.Interfaces
{
    public interface IMovieRepository : IBaseRepository<Movie, Guid>
    {
		Task<bool> ExistesByName(string name);
		Task<Movie> UpdateDetails(Guid id, Movie movie);
		Task<Movie> UpddateThumbnail(Guid id, string thumbnail);
		Task<Movie> SoftDelete(Guid id);
		Task<Movie> UpdateNewAsync(Movie entity);
		Task<Movie> UpdateMovie(Movie movie);
	}
}
