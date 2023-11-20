using OnlineStore.Shared.Clients;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ClientsRelativeUrl = "api/clients";

    public async Task RegisterClientAsync(RegisterClient command)
    {
        var commandUrl = $"{ClientsRelativeUrl}/register";
        await PostAsync(commandUrl, command);
    }

    public async Task ChangeClientPreferences(ChangeUserPreferences command)
    {
        var commandUrl = $"{ClientsRelativeUrl}/change-user-preferences";
        await PutAsync(commandUrl, command);
    }
}
