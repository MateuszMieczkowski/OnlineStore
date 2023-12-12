using Blazored.LocalStorage;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Models;
using OnlineStore.Client.Models.Accounts;
using OnlineStore.Client.Providers;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Services;

public interface IClientService
{
    Task Register(RegisterClient command);

    Task ChangeUserPreferences(ChangePreferencesModel model);

    Task UpsertOrderAddress(UpsertAddressModel model);
    
    Task<UpsertAddressModel?> GetOrderAddress();
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

    public async Task UpsertOrderAddress(UpsertAddressModel model)
    {
        var command = new UpsertOrderAddress(
            Id: model.Id,
            Street: model.Street,
            StreetNumber: model.StreetNumber,
            City: model.City,
            State: model.State,
            PostalCode: model.PostalCode,
            Country: model.Country);

        await _broker.UpsertOrderAddress(command);
    }

    public async Task<UpsertAddressModel?> GetOrderAddress()
    {
        var addressDto = await _broker.GetOrderAddress();
        Console.WriteLine(addressDto);
        if (addressDto is null)
        {
            return null;
        }

        return new UpsertAddressModel
        {
            Id = addressDto.Id,
            City = addressDto.City,
            Country = addressDto.Country,
            PostalCode = addressDto.PostalCode,
            State = addressDto.State,
            Street = addressDto.Street,
            StreetNumber = addressDto.StreetNumber
        };
    }
}
