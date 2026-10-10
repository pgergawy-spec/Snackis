using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface IPrivateMessageService
{
    Task<IEnumerable<PrivateMessageDto>> GetInboxAsync(string userId);
    Task<IEnumerable<PrivateMessageDto>> GetSentAsync(string userId);
    Task<PrivateMessageDto> SendAsync(SendMessageDto sendMessageDto, string senderUserId);
}
