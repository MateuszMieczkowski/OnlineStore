using OnlineStore.Shared.Messages;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<IEnumerable<MessageDto>> GetAllMessagesAsync();

    Task<MessageDto> CreateMessageAsync(CreateMessageRequest request);

    Task<MessageDto> UpdateMessageAsync(Guid id, UpdateMessageRequest request);

    Task<bool> DeleteMessageAsync(Guid id);

    Task<bool> GrantPermissionAsync(Guid messageId, PermissionRequest request);

    Task<bool> RevokePermissionAsync(Guid messageId, PermissionRequest request);
}

