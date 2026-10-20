using Snackis.Models;

namespace Snackis.Services;

public class CommentApiService(IHttpClientFactory httpClientFactory, ILogger<CommentApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), ICommentApiService
{
    public Task<List<CommentViewModel>> GetCommentsAsync(Guid postId)
        => GetAsync<List<CommentViewModel>>($"api/posts/{postId}/comments");

    public Task<CommentViewModel> CreateCommentAsync(CreateCommentViewModel createCommentViewModel)
        => PostAsync<CommentViewModel>("api/comments", new { createCommentViewModel.PostId, createCommentViewModel.Content });

    public Task DeleteCommentAsync(Guid commentId)
        => DeleteAsync($"api/comments/{commentId}");
}
