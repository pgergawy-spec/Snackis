using SnackisAPI.Exceptions;

namespace SnackisAPI.Repositories;

public static class PostRepositoryExtensions
{
    public static async Task EnsureExistsAsync(this IPostRepository postRepository, Guid postId)
    {
        if (await postRepository.GetByIdAsync(postId) is null)
            throw new NotFoundException("Inlägget finns inte.");
    }
}
