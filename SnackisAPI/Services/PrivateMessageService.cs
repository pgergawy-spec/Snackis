using Microsoft.AspNetCore.Identity;
using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Models;
using SnackisAPI.Repositories;

namespace SnackisAPI.Services;

public class PrivateMessageService(IPrivateMessageRepository messageRepository, UserManager<User> userManager, ILogger<PrivateMessageService> logger) : IPrivateMessageService
{
    public async Task<IEnumerable<PrivateMessageDto>> GetInboxAsync(string userId)
        => EntityMappings.ToDtos(await messageRepository.GetInboxAsync(userId));

    public async Task<IEnumerable<PrivateMessageDto>> GetSentAsync(string userId)
        => EntityMappings.ToDtos(await messageRepository.GetSentAsync(userId));

    public async Task<PrivateMessageDto> SendAsync(SendMessageDto sendMessageDto, string senderUserId)
    {
        var receiver = await userManager.FindByNameAsync(sendMessageDto.ReceiverUserName)
            ?? throw new NotFoundException("Mottagaren finns inte.");

        var sender = await userManager.FindByIdAsync(senderUserId)
            ?? throw new NotFoundException("Avsändaren finns inte.");

        if (receiver.Id == sender.Id)
            throw new BadRequestException("Du kan inte skicka meddelanden till dig själv.");

        var message = EntityMappings.ToEntity(sendMessageDto, sender.Id, receiver.Id);
        await messageRepository.AddAsync(message);
        logger.LogInformation("Message {MessageId} sent from user {SenderId} to user {ReceiverId}", message.Id, sender.Id, receiver.Id);

        message.Sender = sender;
        message.Receiver = receiver;
        return EntityMappings.ToDto(message);
    }
}
