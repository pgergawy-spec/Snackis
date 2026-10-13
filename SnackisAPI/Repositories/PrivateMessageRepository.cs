using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class PrivateMessageRepository(SnackisDbContext context) : GenericRepository<PrivateMessage>(context), IPrivateMessageRepository
{
    public async Task<IEnumerable<PrivateMessage>> GetInboxAsync(string userId)
        => await _dbSet.AsNoTracking()
            .Where(message => message.ReceiverUserId == userId)
            .Include(message => message.Sender)
            .Include(message => message.Receiver)
            .OrderByDescending(message => message.Date)
            .ToListAsync();

    public async Task<IEnumerable<PrivateMessage>> GetSentAsync(string userId)
        => await _dbSet.AsNoTracking()
            .Where(message => message.SenderUserId == userId)
            .Include(message => message.Sender)
            .Include(message => message.Receiver)
            .OrderByDescending(message => message.Date)
            .ToListAsync();
}
