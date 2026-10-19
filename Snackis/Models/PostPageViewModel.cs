namespace Snackis.Models;

public class PostPageViewModel
{
    public PostDetailsViewModel Post { get; set; } = new();
    public List<CommentViewModel> Comments { get; set; } = [];
}
