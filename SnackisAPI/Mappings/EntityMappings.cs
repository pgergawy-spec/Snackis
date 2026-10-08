using SnackisAPI.Dtos;
using SnackisAPI.Helpers;
using SnackisAPI.Models;

namespace SnackisAPI.Mappings;

public static class EntityMappings
{
    public static TopicDto ToDto(this Topic topic) => new()
    {
        Id = topic.Id,
        Name = topic.Name,
        UrlName = topic.UrlName,
        ParentId = topic.ParentId
    };

    public static Topic ToEntity(this CreateTopicDto dto) => new()
    {
        Name = dto.Name.Trim(),
        UrlName = dto.Name.ToCleanUrl(),
        ParentId = dto.ParentId
    };

    public static PostDto ToDto(this Post post) => MapPost<PostDto>(post);

    public static PostDetailsDto ToDetailsDto(this Post post)
    {
        var dto = MapPost<PostDetailsDto>(post);
        dto.TopicName = post.Topic?.Name ?? "";
        dto.TopicUrlName = post.Topic?.UrlName ?? "";
        dto.ParentTopicName = post.Topic?.Parent?.Name;
        dto.ParentTopicUrlName = post.Topic?.Parent?.UrlName;
        return dto;
    }

    public static Post ToEntity(this CreatePostDto dto, string userId) => new()
    {
        Title = dto.Title.Trim(),
        Content = dto.Content.Trim(),
        TopicId = dto.TopicId,
        UserId = userId,
        Date = DateTime.UtcNow
    };

    public static CommentDto ToDto(this Comment comment) => new()
    {
        Id = comment.Id,
        Content = comment.Content,
        Date = comment.Date,
        PostId = comment.PostId,
        AuthorId = comment.UserId,
        AuthorName = comment.User?.UserName ?? "",
        AuthorPictureUrl = comment.User?.ProfilePictureUrl
    };

    public static Comment ToEntity(this CreateCommentDto dto, string userId) => new()
    {
        Content = dto.Content.Trim(),
        PostId = dto.PostId,
        UserId = userId,
        Date = DateTime.UtcNow
    };

    public static PrivateMessageDto ToDto(this PrivateMessage message) => new()
    {
        Id = message.Id,
        Content = message.Content,
        Date = message.Date,
        SenderId = message.SenderUserId,
        SenderName = message.Sender?.UserName ?? "",
        SenderPictureUrl = message.Sender?.ProfilePictureUrl,
        ReceiverId = message.ReceiverUserId,
        ReceiverName = message.Receiver?.UserName ?? ""
    };

    public static PrivateMessage ToEntity(this SendMessageDto dto, string senderUserId, string receiverUserId) => new()
    {
        Content = dto.Content.Trim(),
        SenderUserId = senderUserId,
        ReceiverUserId = receiverUserId,
        Date = DateTime.UtcNow
    };

    public static ReportDto ToDto(this Report report) => new()
    {
        Id = report.Id,
        Date = report.Date,
        Reason = report.Reason,
        IsReviewed = report.IsReviewed,
        ReporterName = report.User?.UserName ?? "",
        PostId = report.PostId,
        PostTitle = report.Post?.Title ?? "",
        PostAuthorName = report.Post?.User?.UserName ?? "",
        TopicUrlName = report.Post?.Topic?.UrlName ?? ""
    };

    public static Report ToEntity(this CreateReportDto dto, string userId) => new()
    {
        Reason = dto.Reason.Trim(),
        PostId = dto.PostId,
        UserId = userId,
        IsReviewed = false,
        Date = DateTime.UtcNow
    };

    public static UserDto ToDto(this User user, IEnumerable<string> roles) => new()
    {
        Id = user.Id,
        UserName = user.UserName ?? "",
        Email = user.Email ?? "",
        ProfilePictureUrl = user.ProfilePictureUrl,
        Roles = roles.ToList()
    };

    private static T MapPost<T>(Post post) where T : PostDto, new() => new()
    {
        Id = post.Id,
        Title = post.Title,
        UrlTitle = post.Title.ToCleanUrl(),
        Content = post.Content,
        Date = post.Date,
        TopicId = post.TopicId,
        AuthorId = post.UserId,
        AuthorName = post.User?.UserName ?? "",
        AuthorPictureUrl = post.User?.ProfilePictureUrl
    };
}
