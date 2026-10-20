using SnackisAPI.Exceptions;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public static class PostRepositoryChecks
{
    public static async Task<Post> EnsureExistsAsync(IPostRepository postRepository, Guid postId)
        => await postRepository.GetByIdAsync(postId)
            ?? throw new NotFoundException("Inlägget finns inte.");
}
