using Snackis.Models;

namespace Snackis.Services;

public interface IMessageApiService
{
    Task<List<PrivateMessageViewModel>> GetInboxAsync();
    Task<List<PrivateMessageViewModel>> GetSentAsync();
    Task<PrivateMessageViewModel> SendMessageAsync(SendMessageViewModel sendMessageViewModel);
}
