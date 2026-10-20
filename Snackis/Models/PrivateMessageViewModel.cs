namespace Snackis.Models;

public class PrivateMessageViewModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = "";
    public DateTime Date { get; set; }
    public string SenderId { get; set; } = "";
    public string SenderName { get; set; } = "";
    public string? SenderPictureUrl { get; set; }
    public string ReceiverId { get; set; } = "";
    public string ReceiverName { get; set; } = "";
    public string? ReceiverPictureUrl { get; set; }
}
