using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class PrivateMessageRepository : GenericRepository<PrivateMessage>, IPrivateMessageRepository
{
    public PrivateMessageRepository(SnackisDbContext context) : base(context)
    {

    }
    public async Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId)
    => await _dbSet.Where(message => message.ReceiverUserId == userId)
        .Include(message => message.Sender)
        .OrderByDescending(message => message.Date).ToListAsync();

    public async Task<IEnumerable<PrivateMessage>> GetSentAsync(string userId)
        => await _dbSet.Where(message => message.SenderUserId == userId)
            .Include(message => message.Receiver)
            .OrderByDescending(message => message.Date).ToListAsync();
}
