using Snackis.Models;

namespace Snackis.Services;

public class PostApiService(IHttpClientFactory httpClientFactory, ILogger<PostApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), IPostApiService
{
    public Task<List<PostViewModel>> GetPostsByTopicAsync(string topicUrlName)
        => GetAsync<List<PostViewModel>>($"api/topics/{Uri.EscapeDataString(topicUrlName)}/posts");

    public Task<PostDetailsViewModel> GetPostAsync(Guid postId)
        => GetAsync<PostDetailsViewModel>($"api/posts/{postId}");

    public Task<PostViewModel> CreatePostAsync(CreatePostViewModel createPostViewModel)
        => PostAsync<PostViewModel>("api/posts", new { createPostViewModel.Title, createPostViewModel.Content, createPostViewModel.TopicId });

    public Task DeletePostAsync(Guid postId)
        => DeleteAsync($"api/posts/{postId}");
}
