using NetFilm.Application.DTOs.CommentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAll();
        Task<CommentDto> GetById(Guid id);
        Task<IEnumerable<CommentDto>> GetCommentByMovieId(Guid movieId);
        Task<IEnumerable<CommentDto>> GetCommentsByCommentId(Guid commentId);
        Task<CommentDto> Add(AddCommentDto addCommentDto);
        Task<CommentDto> Update(Guid id, UpdateCommentDto updateCommentDto);
        Task<ReplyDto> Reply(AddReplyDto reply);
        Task<IEnumerable<ReplyDto>> GetAllRepliesByCommentId(Guid commentId);
        Task<bool> HardDelete(Guid id);
        Task<CommentDto> SoftDelete(Guid id);
    }
}
