using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Persistence.Repositories
{
	public class MovieRepository : BaseRepository<Movie, Guid>, IMovieRepository
	{
		public MovieRepository(NetFilmDbContext context) : base(context)
		{
		}
	}
}
