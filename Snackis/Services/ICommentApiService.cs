using Snackis.Models;

namespace Snackis.Services;

public interface ICommentApiService
{
    Task<List<CommentViewModel>> GetCommentsAsync(Guid postId);
    Task<CommentViewModel> CreateCommentAsync(CreateCommentViewModel createCommentViewModel);
    Task DeleteCommentAsync(Guid commentId);
}
