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
	public class CommentRepository : BaseRepository<Comment, Guid>, ICommentRepository
	{
		public CommentRepository(NetFilmDbContext context) : base(context)
		{
		}
	}
}
