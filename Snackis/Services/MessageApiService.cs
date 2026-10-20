using Snackis.Models;

namespace Snackis.Services;

public class MessageApiService(IHttpClientFactory httpClientFactory, ILogger<MessageApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), IMessageApiService
{
    public Task<List<PrivateMessageViewModel>> GetInboxAsync()
        => GetAsync<List<PrivateMessageViewModel>>("api/messages/inbox");

    public Task<List<PrivateMessageViewModel>> GetSentAsync()
        => GetAsync<List<PrivateMessageViewModel>>("api/messages/sent");

    public Task<PrivateMessageViewModel> SendMessageAsync(SendMessageViewModel sendMessageViewModel)
        => PostAsync<PrivateMessageViewModel>("api/messages", new { sendMessageViewModel.ReceiverUserName, sendMessageViewModel.Content });
}
