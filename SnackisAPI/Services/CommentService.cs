using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Helpers;
using SnackisAPI.Mappings;
using SnackisAPI.Repositories;

namespace SnackisAPI.Services;

public class CommentService(ICommentRepository commentRepository, IPostRepository postRepository, ILogger<CommentService> logger) : ICommentService
{
    public async Task<IEnumerable<CommentDto>> GetByPostAsync(Guid postId)
    {
        await PostRepositoryChecks.EnsureExistsAsync(postRepository, postId);

        return EntityMappings.ToDtos(await commentRepository.GetByPostAsync(postId));
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto createCommentDto, string userId)
    {
        await PostRepositoryChecks.EnsureExistsAsync(postRepository, createCommentDto.PostId);

        var comment = EntityMappings.ToEntity(createCommentDto, userId);
        await commentRepository.AddAsync(comment);
        logger.LogInformation("Comment {CommentId} created by user {UserId} on post {PostId}", comment.Id, userId, comment.PostId);

        var createdComment = await commentRepository.GetByIdWithAuthorAsync(comment.Id)
            ?? throw new NotFoundException("Kommentaren finns inte.");

        return EntityMappings.ToDto(createdComment);
    }

    public async Task DeleteAsync(Guid id, string userId, bool isAdmin)
    {
        var comment = await commentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Kommentaren finns inte.");

        AccessRules.EnsureOwnerOrAdmin(comment.UserId, userId, isAdmin, "Du kan bara ta bort dina egna kommentarer.");

        await commentRepository.DeleteAsync(comment);
        logger.LogInformation("Comment {CommentId} deleted by user {UserId}", comment.Id, userId);
    }
}
