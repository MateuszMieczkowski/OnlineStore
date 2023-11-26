using Blazored.LocalStorage;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Models;
using OnlineStore.Client.Providers;
using OnlineStore.Shared.Clients;

namespace OnlineStore.Client.Services;

public interface IClientService
{
    Task Register(RegisterClient command);

    Task ChangeUserPreferences(ChangePreferencesModel model);
}

public class ClientService : IClientService
{
    private readonly IApiBroker _broker;
    private readonly ICallContext _callContext;
    private readonly ILocalStorageService _localStorage;

    public ClientService(IApiBroker broker, ICallContext callContext, ILocalStorageService localStorage)
    {
        _broker = broker ?? throw new ArgumentNullException(nameof(broker));
        _callContext = callContext ?? throw new ArgumentNullException(nameof(callContext));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public async Task Register(RegisterClient command)
    {
        await _broker.RegisterClientAsync(command);
    }

    public async Task ChangeUserPreferences(ChangePreferencesModel model)
    {
        var userId = await _callContext.GetUserId();
        var command = new ChangeUserPreferences(
            UserId: userId,
            UiThemeDto: model.UiTheme,
            DisplayedPriceDto: model.DisplayedPrice,
            IsSubscribedToNewsletter: model.IsSubscribedToNewsLetter,
            PageSize: model.PageSize);

        await _broker.ChangeClientPreferences(command);

        await _localStorage.SetItemAsync(LocalStorageKeys.UserPreferences,
            new UserPreferencesDto(
                model.UiTheme,
                model.DisplayedPrice,
                model.IsSubscribedToNewsLetter,
                model.PageSize));
    }
}
