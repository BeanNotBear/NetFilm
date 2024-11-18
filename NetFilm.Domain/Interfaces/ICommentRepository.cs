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
		Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<IEnumerable<Comment>> GetAllCommentsByMovieIdAsync(Guid movieId);
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<IEnumerable<Comment>> GetByIdCommentAsync(Guid commentId);
        Task<Comment> ReplyCommentAsync(Comment reply);
        Task<Comment> SoftDeleteAsync(Guid id);
		Task<IEnumerable<Comment>> GetAllRepliesByCommentIdAsync(Guid id);
        Task<IEnumerable<Comment>> GetCommentPagedResultAsync(int pageSize, int pageIndex, string searchTerm, string sortBy, bool ascending);
    }
}
