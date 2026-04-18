using OnlineStore.Client.Brokers.API;
using OnlineStore.Shared.Messages;

namespace OnlineStore.Client.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageDto>> GetAllMessagesAsync();
    Task<MessageDto> CreateMessageAsync(CreateMessageRequest request);
    Task<MessageDto> UpdateMessageAsync(Guid id, UpdateMessageRequest request);
    Task<bool> DeleteMessageAsync(Guid id);
    Task<bool> GrantPermissionAsync(Guid messageId, PermissionRequest request);
    Task<bool> RevokePermissionAsync(Guid messageId, PermissionRequest request);
}

public class MessageService : IMessageService
{
    private readonly IApiBroker _broker;

    public MessageService(IApiBroker broker)
    {
        _broker = broker;
    }

    public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
    {
        return await _broker.GetAllMessagesAsync();
    }

    public async Task<MessageDto> CreateMessageAsync(CreateMessageRequest request)
    {
        return await _broker.CreateMessageAsync(request);
    }

    public async Task<MessageDto> UpdateMessageAsync(Guid id, UpdateMessageRequest request)
    {
        return await _broker.UpdateMessageAsync(id, request);
    }

    public async Task<bool> DeleteMessageAsync(Guid id)
    {
        return await _broker.DeleteMessageAsync(id);
    }

    public async Task<bool> GrantPermissionAsync(Guid messageId, PermissionRequest request)
    {
        return await _broker.GrantPermissionAsync(messageId, request);
    }

    public async Task<bool> RevokePermissionAsync(Guid messageId, PermissionRequest request)
    {
        return await _broker.RevokePermissionAsync(messageId, request);
    }
}

