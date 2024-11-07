using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Interfaces
{
	public interface ICommentRepository : IBaseRepository<Comment, Guid>
	{
		Task<IEnumerable<Comment>> GetByIdMovieAsync(Guid movieId);
        Task<IEnumerable<Comment>> GetByIdCommentAsync(Guid commentId);
        Task<Comment> ReplyCommentAsync(Guid commentId,Comment reply);
	}
}
