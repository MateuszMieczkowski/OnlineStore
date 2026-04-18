using OnlineStore.Shared.Messages;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string MessagesRelativeUrl = "/api/messages";

    public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
    {
        return await GetAsync<IEnumerable<MessageDto>>(MessagesRelativeUrl);
    }

    public async Task<MessageDto> CreateMessageAsync(CreateMessageRequest request)
    {
        return await PostAsync<CreateMessageRequest, MessageDto>(MessagesRelativeUrl, request);
    }

    public async Task<MessageDto> UpdateMessageAsync(Guid id, UpdateMessageRequest request)
    {
        var url = $"{MessagesRelativeUrl}/{id}";
        return await PatchAsync<UpdateMessageRequest, MessageDto>(url, request);
    }

    public async Task<bool> DeleteMessageAsync(Guid id)
    {
        var url = $"{MessagesRelativeUrl}/{id}";
        return await DeleteAsync(url);
    }

    public async Task<bool> GrantPermissionAsync(Guid messageId, PermissionRequest request)
    {
        var url = $"{MessagesRelativeUrl}/{messageId}/permissions/grant";
        return await PostAsync(url, request);
    }

    public async Task<bool> RevokePermissionAsync(Guid messageId, PermissionRequest request)
    {
        var url = $"{MessagesRelativeUrl}/{messageId}/permissions/revoke";
        return await PostAsync(url, request);
    }
}

