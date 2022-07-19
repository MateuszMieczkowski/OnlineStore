using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Client.Brokers.API
{
    public partial interface IApiBroker
    {
        Task<T> GetAsync<T>(string relativeUrl);
        Task<T> GetWithAuthAsync<T>(string relativeUrl);

        Task<bool> PostAsync<T>(string relativeUrl, T content);
        Task<TDto> PostAsync<TPostDto, TDto>(string relativeUrl, TPostDto content);


        Task<bool> PutAsync<T>(string relativeUrl, T content);
        Task<TDto> PutAsync<TPutDto, TDto>(string relativeUrl, TPutDto content);


        Task<bool> DeleteAsync(string relativeUrl);
    }
}
