namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<T> GetAsync<T>(string relativeUrl);

    Task<bool> PostAsync<T>(string relativeUrl, T content);

    Task<TDto> PostAsync<TPostDto, TDto>(string relativeUrl, TPostDto content);

    Task<bool> PutAsync<T>(string relativeUrl, T content);

    Task PutAsync(string relativeUrl);

    Task<TDto> PutAsync<TPutDto, TDto>(string relativeUrl, TPutDto content);

    Task<bool> DeleteAsync(string relativeUrl);
}