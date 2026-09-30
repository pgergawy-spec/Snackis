namespace SnackisAPI.Models;

public class PrivateMessage
{
    public Guid Id { get; set; }
    public string SenderUserId { get; set; } = "";
    public User? Sender { get; set; }
    public string ReceiverUserId { get; set; } = "";
    public User? Receiver { get; set; }
    public string Content { get; set; } = "";
    public DateTime Date { get; set; }
}
