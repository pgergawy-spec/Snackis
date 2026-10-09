using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetByPostAsync(Guid postId);
    Task<CommentDto> CreateAsync(CreateCommentDto createCommentDto, string userId);
    Task DeleteAsync(Guid id, string userId, bool isAdmin);
}
