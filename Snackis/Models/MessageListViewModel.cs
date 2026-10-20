namespace Snackis.Models;

public class MessageListViewModel
{
    public string Heading { get; set; } = "";
    public bool ShowSender { get; set; }
    public List<PrivateMessageViewModel> Messages { get; set; } = [];
}
