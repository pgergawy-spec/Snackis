using SnackisAPI.Exceptions;

namespace SnackisAPI.Repositories;

public static class PostRepositoryChecks
{
    public static async Task EnsureExistsAsync(IPostRepository postRepository, Guid postId)
    {
        if (await postRepository.GetByIdAsync(postId) is null)
            throw new NotFoundException("Inlägget finns inte.");
    }
}
