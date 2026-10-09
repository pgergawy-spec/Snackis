using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public interface IPrivateMessageRepository : IRepository<PrivateMessage>
{
    Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId);
    Task<IEnumerable<PrivateMessage>> GetSentAsync(string userId);
}
