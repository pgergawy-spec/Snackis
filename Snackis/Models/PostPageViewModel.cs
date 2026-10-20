namespace Snackis.Models;

public class PostPageViewModel
{
    public PostDetailsViewModel Post { get; set; } = new();
    public List<CommentViewModel> Comments { get; set; } = [];
    public CreateCommentViewModel NewComment { get; set; } = new();
    public string? CurrentUserId { get; set; }
    public bool IsAdmin { get; set; }

    public bool CanDelete(string authorId)
        => IsAdmin || (CurrentUserId is not null && authorId == CurrentUserId);
}
